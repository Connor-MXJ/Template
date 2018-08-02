//using MXJ.Core.Application;
//using MXJ.Core.Repositories;

namespace MXJ.Core.Application.Commands
{
   /// <summary>
   /// 命令处理器基类
   /// </summary>
   /// <typeparam name="TCommand"></typeparam>
    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {

        #region 方法
      /// <summary>
      /// 处理命令
      /// </summary>
      /// <param name="command"></param>
        public abstract void Handle(TCommand command);
        #endregion

        #region Protected Properties
        /// <summary>
        /// 领域仓储
        /// </summary>
        //protected virtual IDomainRepository DomainRepository
        //{
        //    get { return AppRuntime.Instance.CurrentApplication.ObjectContainer.GetService<IDomainRepository>(); }
        //}
        #endregion

    }
}
