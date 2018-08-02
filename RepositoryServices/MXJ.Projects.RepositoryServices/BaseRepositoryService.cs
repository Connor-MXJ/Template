using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using MXJ.Core.Infrastructure.Caching;
using MXJ.Core.Infrastructure.Engine;
using MXJ.Core.Infrastructure.Logging;
using MXJ.Projects.Domain.Context;
using MXJ.Projects.IRepositoryServices;
using PropertyAttributes = System.Reflection.PropertyAttributes;

namespace MXJ.Projects.RepositoryServices
{
    public class BaseRepositoryService : IBaseRepositoryService
    {
        public BaseRepositoryService()
        {
            BusinessRepositoryContext = EngineContext.Current.DependencyContainer.Resolve<IDomainRepositoryContext>();
            this.CacheManager = EngineContext.Current.DependencyContainer.Resolve<ICacheFactory>().GetCacheManager();
            this.Logger = EngineContext.Current.DependencyContainer.Resolve<ILoggerFactory>().CreateLogger();
        }

        protected IDomainRepositoryContext BusinessRepositoryContext { get; }
        /// <summary>
        /// Gets the cache manager.
        /// </summary>
        protected ICacheManager CacheManager { get; private set; }
        /// <summary>
        ///     Gets the logger.
        /// </summary>
        private ILogger Logger { get; set; }

        #region SQL操作

        /// <summary>
        ///     查询
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="query">sql执行语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>返回指定类型</returns>
        protected List<T> Query<T>(string query, IDictionary<string, object> parameters = null)
        {
            var list = default(List<T>);
            if (!string.IsNullOrEmpty(query))
            {
                if (null != parameters)
                    list =
                        BusinessRepositoryContext.DbContext.Database.SqlQuery<T>(query, GetSqlParameters(parameters))
                            .ToList();
                else
                    list = BusinessRepositoryContext.DbContext.Database.SqlQuery<T>(query).ToList();
            }

            return list;
        }

        /// <summary>
        ///     查询
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="query">sql执行语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>返回指定类型</returns>
        protected dynamic Query(string query, IDictionary<string, object> parameters = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                return null;
            }

            if (null != parameters)
            {
                return DynamicSqlQuery(BusinessRepositoryContext.DbContext.Database, query,
                    GetSqlParameters(parameters)); 
            }
            else
            {
                return DynamicSqlQuery(BusinessRepositoryContext.DbContext.Database, query); 
            }
        }

        private IEnumerable DynamicSqlQuery(Database database, string sql, params object[] parameters)
        {
            var builder = CreateTypeBuilder("MyDynamicAssembly", "MyDynamicModule", "MyDynamicType");
            using (IDbCommand command = database.Connection.CreateCommand())
            {
                try
                {
                    database.Connection.Open();
                    command.CommandText = sql;
                    command.CommandTimeout = command.Connection.ConnectionTimeout;
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }
                    using (var reader = command.ExecuteReader())
                    {
                        var schema = reader.GetSchemaTable();
                        if (schema != null)
                        {
                            foreach (DataRow row in schema.Rows)
                            {
                                var name = (string)row["ColumnName"];
                                var type = (Type)row["DataType"];
                                if (type != typeof(string) && (bool)row.ItemArray[schema.Columns.IndexOf("AllowDbNull")])
                                {
                                    type = typeof(Nullable<>).MakeGenericType(type);
                                }
                                CreateAutoImplementedProperty(builder, name, type);
                            }
                        }
                    }
                }
                finally
                {
                    database.Connection.Close();
                    command.Parameters.Clear();
                }
            }
            var resultType = builder.CreateType();
            return database.SqlQuery(resultType, sql, parameters);
        }

        private TypeBuilder CreateTypeBuilder(string assemblyName, string moduleName, string typeName)
        {
            var typeBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(assemblyName),
                    AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName)
                .DefineType(typeName, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        private void CreateAutoImplementedProperty(TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string privateFieldPrefix = "m_";
            const string getterPrefix = "get_";
            const string setterPrefix = "set_";

            // Generate the field.
            var fieldBuilder = builder.DefineField(
                string.Concat(privateFieldPrefix, propertyName),
                propertyType, FieldAttributes.Private);

            // Generate the property
            var propertyBuilder = builder.DefineProperty(
                propertyName, PropertyAttributes.HasDefault, propertyType, null);

            // Property getter and setter attributes.
            var propertyMethodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig;

            // Define the getter method.
            var getterMethod = builder.DefineMethod(
                string.Concat(getterPrefix, propertyName),
                propertyMethodAttributes, propertyType, Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            var getterIlCode = getterMethod.GetILGenerator();
            getterIlCode.Emit(OpCodes.Ldarg_0);
            getterIlCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterIlCode.Emit(OpCodes.Ret);

            // Define the setter method.
            var setterMethod = builder.DefineMethod(
                string.Concat(setterPrefix, propertyName),
                propertyMethodAttributes, null, new[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            var setterIlCode = setterMethod.GetILGenerator();
            setterIlCode.Emit(OpCodes.Ldarg_0);
            setterIlCode.Emit(OpCodes.Ldarg_1);
            setterIlCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterIlCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }

        /// <summary>
        ///     SQL执行
        /// </summary>
        /// <param name="query">sql执行语句</param>
        /// <param name="parameters">sql参数</param>
        /// <returns>是否执行成功</returns>
        protected bool Execute(string query, IDictionary<string, object> parameters = null)
        {
            var rows = -1;
            if (!string.IsNullOrEmpty(query))
            {
                if (null != parameters)
                    rows = BusinessRepositoryContext.DbContext.Database.ExecuteSqlCommand(query,
                        GetSqlParameters(parameters));
                else
                    rows = BusinessRepositoryContext.DbContext.Database.ExecuteSqlCommand(query);
            }
            return rows >= 0;
        }

        /// <summary>
        ///     获取sql参数
        /// </summary>
        /// <param name="parameters">参数集合</param>
        /// <returns></returns>
        private object[] GetSqlParameters(IDictionary<string, object> parameters)
        {
            return parameters?.Select(parameter => new SqlParameter(parameter.Key, parameter.Value)).ToArray();
        }

        #endregion

        #region 日志记录
        public void WriteLogInfo(string info)
        {
            Logger.Information(info);
        }

        public void WriteDebugInfo(string info)
        {
            Logger.Debug(info);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="error"></param>
        public void WriteLogError(string error)
        {
            Logger.Error(error);
        }

        public string WriteLogException(Exception exc)
        {
            string error = GetInnerExceptionErrorMessage(exc);
            Logger.Error(error, exc);
            return error;
        }

        private string GetInnerExceptionErrorMessage(Exception exc)
        {
            var error = string.Empty;
            SetInnerExceptionErrorMessage(exc, ref error);
            return error;
        }

        private void SetInnerExceptionErrorMessage(Exception exc, ref string error)
        {
            if (null != exc)
            {
                error += exc.Message + ";";
                SetInnerExceptionErrorMessage(exc.InnerException, ref error);
            }
        }
        #endregion
    }
}