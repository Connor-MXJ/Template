using System;
using System.Collections.Generic;

namespace MXJ.Core.Infrastructure.IOC
{
    /// <summary>
    /// 对象管理器
    /// </summary>
    public interface IDependencyContainer : IDisposable
    {
        /// <summary>
        /// 类型是否注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool IsRegistered<T>();

        /// <summary>
        /// 类型是否注册
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool IsRegistered(Type typeToCheck);

        /// <summary>
        /// 获取某类型实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        /// 获取某类型实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// 获取某类型所有实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// 获取某类型所有实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<object> ResolveAll(Type type);

        /// <summary>
        /// 注册单态类型
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TO"></typeparam>
        void RegisterSingleton<TFrom, TO>() where TO : TFrom;

        /// <summary>
        /// 注册单态类型
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        void RegisterSingleton(Type fromType, Type toType);

        /// <summary>
        /// 注册每次请求实例化一个对象
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TO"></typeparam>
        void RegisterPerRequest<TFrom, TO>() where TO : TFrom;

        /// <summary>
        /// 注册每次请求实例化一个对象
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        void RegisterPerRequest(Type fromType, Type toType);

        /// <summary>
        /// 注册类型（每次调用实例化一个）
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TO"></typeparam>
        void Register<TFrom, TO>() where TO : TFrom;

        /// <summary>
        /// 注册类型（每次调用实例化一个）
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        void Register(Type fromType, Type toType);

        /// <summary>
        /// 创建子容器
        /// </summary>
        /// <returns></returns>
        IDependencyContainer CreateChildContainer();

        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="o"></param>
        void Teardown(object o);
    }
}