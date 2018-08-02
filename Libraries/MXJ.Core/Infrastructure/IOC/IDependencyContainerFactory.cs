namespace MXJ.Core.Infrastructure.IOC
{
    /// <summary>
    /// 对象管理器工厂
    /// </summary>
    public interface IDependencyContainerFactory
    {
        /// <summary>
        /// 创建对象管理器
        /// </summary>
        /// <returns></returns>
        IDependencyContainer CreateInstance();
    }
}