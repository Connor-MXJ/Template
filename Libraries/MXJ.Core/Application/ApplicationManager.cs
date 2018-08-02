using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MXJ.Core.Infrastructure.Engine;
using MXJ.Core.Application.Services;

namespace MXJ.Core.Application
{
    /// <summary>
    /// 应用程序管理器
    /// </summary>
    public static class ApplicationManager
    {
        /// <summary>
        /// 注册业务层
        /// </summary>
        /// <param name="domainAssemblyFile"></param>
        public static void RegisterBussinessService(string interfaceAssembly, string implementAssembly)
        {
            var iServiceType = from t in Assembly.Load(interfaceAssembly).GetTypes()
                               where (typeof(IBussinessService).IsAssignableFrom(t) && ((Type)t).IsInterface)
                               select t;
            foreach (var i in iServiceType)
            {
                var serviceTypes = from t in Assembly.Load(implementAssembly).GetTypes()
                                   where (((Type)i).IsAssignableFrom(t) && ((Type)t).IsClass)
                                   select t;
                foreach (var t in serviceTypes)
                {
                    EngineContext.Current.DependencyContainer.Register(i, t);
                }
            }
        }

        /// <summary>
        /// 注册持久化层
        /// </summary>
        /// <param name="domainAssemblyFile"></param>
        public static void RegisterRepositoryService(string interfaceAssembly, string implementAssembly)
        {
            var iServiceType = from t in Assembly.Load(interfaceAssembly).GetTypes()
                               where (typeof(IRepositoryService).IsAssignableFrom(t) && ((Type)t).IsInterface)
                               select t;
            foreach (var i in iServiceType)
            {
                var serviceTypes = from t in Assembly.Load(implementAssembly).GetTypes()
                                   where (((Type)i).IsAssignableFrom(t) && ((Type)t).IsClass)
                                   select t;
                foreach (var t in serviceTypes)
                {
                    EngineContext.Current.DependencyContainer.Register(i, t);
                }
            }
        }
    }
}
