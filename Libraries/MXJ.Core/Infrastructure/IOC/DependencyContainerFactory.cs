using System;
using System.Reflection;
using MXJ.Core.Utility.Helper;

namespace MXJ.Core.Infrastructure.IOC
{
    /// <summary>
    /// 对象管理器工厂
    /// </summary>
    public class DependencyContainerFactory : IDependencyContainerFactory
    {
        private readonly Type _resolverType;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resolverTypeName"></param>
        public DependencyContainerFactory(string resolverTypeName)
        {
            ArgumentChecker.IsNotEmpty(resolverTypeName, "resolverTypeName");
            _resolverType = Type.GetType(resolverTypeName, true, true);
            //Assembly asm=  Assembly.Load(resolverTypeName.Split(',')[1]);
            //_resolverType = asm.GetType(resolverTypeName.Split(',')[0], true, true);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DependencyContainerFactory()
            : this(ConfigHelper.DependencyResolverTypeName)
        {
        }

        /// <summary>
        /// 创建对象管理器实例
        /// </summary>
        /// <returns></returns>
        public IDependencyContainer CreateInstance()
        {
            return Activator.CreateInstance(_resolverType) as IDependencyContainer;
        }
    }
}