using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Domain.Repositories;
using MXJ.Core.Domain.Services;
using MXJ.Core.Infrastructure.Engine;

namespace MXJ.Core.Domain
{
    /// <summary>
    /// 领域管理器
    /// </summary>
    public static class DomainManager
    { 
        /// <summary>
        /// 注册领域
        /// </summary>
        /// <param name="domainAssembly"></param>
        /// <param name="domainDataAssembly"></param>
        public static void RegisterDomain(string domainAssembly, string domainDataAssembly)
        {
            RegisterService(domainAssembly);
            RegisterRepositoryContext(domainAssembly, domainDataAssembly);
            RegisterRepository(domainAssembly, domainDataAssembly);
        }

        /// <summary>
        /// 注册领域服务
        /// </summary>
        /// <param name="domainAssembly"></param>
        private static void RegisterService(string domainAssembly)
        {
            var iServiceType = from t in Assembly.Load(domainAssembly).GetTypes()
                               where (typeof(IDomainService).IsAssignableFrom(t) && ((Type)t).IsInterface)
                               select t;
            foreach (var i in iServiceType)
            {
                var serviceTypes = from t in Assembly.Load(domainAssembly).GetTypes()
                                   where (((Type)i).IsAssignableFrom(t) && ((Type)t).IsClass)
                                   select t;
                foreach (var t in serviceTypes)
                {
                    EngineContext.Current.DependencyContainer.Register(i, t);
                }
            }
        }

        /// <summary>
        /// 注册仓储上下文
        /// </summary>
        /// <param name="domainAssembly"></param>
        /// <param name="domainDataAssembly"></param>
        private static void RegisterRepositoryContext(string domainAssembly, string domainDataAssembly)
        {
            var iRepositoryContextType = from t in Assembly.Load(domainAssembly).GetTypes()
                                         where (typeof(IRepositoryContext).IsAssignableFrom(t) && ((Type)t).IsInterface)
                                         select t;
            foreach (var i in iRepositoryContextType)
            {
                var repositoryContextTypes = from t in Assembly.Load(domainDataAssembly).GetTypes()
                                             where (((Type)i).IsAssignableFrom(t) && ((Type)t).IsClass)
                                             select t;
                foreach (var t in repositoryContextTypes)
                {
                    EngineContext.Current.DependencyContainer.RegisterPerRequest(i, t);
                }
            }
        }

        /// <summary>
        /// 注册仓储
        /// </summary>
        /// <param name="domainDataAssembly"></param>
        private static void RegisterRepository(string domainAssembly, string domainDataAssembly)
        {
            var iRepositoryType = from t in Assembly.Load(domainAssembly).GetTypes()
                                  where (t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<,>)) && t.IsInterface)
                                  select t;
            foreach (var i in iRepositoryType)
            {
                var repositoryTypes = from t in Assembly.Load(domainDataAssembly).GetTypes()
                                      where (t.GetInterfaces().Any(s => s.IsGenericType && s.GetGenericTypeDefinition() == i) && t.IsClass)
                                      select t;
                foreach (var t in repositoryTypes)
                {
                    EngineContext.Current.DependencyContainer.Register(i, t);
                }
            }
        }
    }
}