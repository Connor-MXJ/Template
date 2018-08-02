using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using MXJ.Core.Infrastructure.IOC;
using MXJ.Core.Infrastructure.Dispose;
using System.Collections.ObjectModel;
using MXJ.Core.Utility.Helper;
using System.Configuration;

namespace MXJ.Core.IOC.Unity
{
    /// <summary>
    /// 对象管理器
    /// </summary>
    public class UnityDependencyContainer : DisposableResource, IDependencyContainer
    {
        private readonly IUnityContainer _container;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UnityDependencyContainer()
            : this(new UnityContainer())
        {
            //UnityConfigurationSection configuration = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            //configuration.Configure(container);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container"></param>
        public UnityDependencyContainer(IUnityContainer unityContainer)
        {
            ArgumentChecker.IsNotNull(unityContainer, "container");

            _container = unityContainer;
        }

        /// <summary>
        /// 类型是否注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool IsRegistered<T>()
        {
            return _container.IsRegistered<T>();
        }

        /// <summary>
        /// 类型是否注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool IsRegistered(Type typeToCheck)
        {
            return _container.IsRegistered(typeToCheck);
        }


        /// <summary>
        /// 获取某类型实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// 获取某类型实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        /// <summary>
        /// 获取某类型所有实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return _container.ResolveAll<T>();
        }

        /// <summary>
        /// 获取某类型所有实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            return _container.ResolveAll(type);
        }

        /// <summary>
        /// 注册单态类型
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TO"></typeparam>
        public void RegisterSingleton<TFrom, TO>() where TO : TFrom
        {
            _container.RegisterType<TFrom, TO>(new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// 注册单态类型
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        public void RegisterSingleton(Type fromType, Type toType)
        {
            _container.RegisterType(fromType, toType, new ContainerControlledLifetimeManager());
        }

        /// <summary>
        /// 注册每次请求实例化一个对象
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TO"></typeparam>
        public void RegisterPerRequest<TFrom, TO>() where TO : TFrom
        {
            _container.RegisterType<TFrom, TO>(new UnityPerRequestLifetimeManager());
        }

        /// <summary>
        /// 注册每次请求实例化一个对象
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        public void RegisterPerRequest(Type fromType, Type toType)
        {
            _container.RegisterType(fromType, toType, new UnityPerRequestLifetimeManager());
        }

        /// <summary>
        /// 注册类型（每次调用实例化一个）
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TO"></typeparam>
        public void Register<TFrom, TO>() where TO : TFrom
        {
            _container.RegisterType<TFrom, TO>(new TransientLifetimeManager());
        }

        /// <summary>
        /// 注册类型（每次调用实例化一个）
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        public void Register(Type fromType, Type toType)
        {
            _container.RegisterType(fromType, toType, new TransientLifetimeManager());
        }

        /// <summary>
        /// 创建子容器
        /// </summary>
        /// <returns></returns>
        public IDependencyContainer CreateChildContainer()
        {
            return new UnityDependencyContainer();
        }

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="o"></param>
        public void Teardown(object o)
        {
            _container.Teardown(o);
        }

        /// <summary>
        /// 析构方法
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _container.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}