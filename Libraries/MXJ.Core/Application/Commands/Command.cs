using System;

namespace MXJ.Core.Application.Commands
{
    /// <summary>
    ///     命令基类
    /// </summary>
    [Serializable]
    public class Command : ICommand
    {
        #region IEntity Members

        /// <summary>
        ///     Gets or sets the identifier of the command instance.
        /// </summary>
        public virtual Guid AggregateRootId { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///     获取hash值
        /// </summary>
        /// <returns></returns>
        /// <summary>
        ///     判断对象是否相同
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            if (obj == null)
                return false;
            var other = obj as Command;
            if (other == null)
                return false;
            return AggregateRootId == other.AggregateRootId;
        }

        #endregion

        #region Ctor

        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        public Command()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <c>Command</c> class.
        /// </summary>
        /// <param name="id">The identifier which identifies a single command instance.</param>
        public Command(Guid id)
        {
            AggregateRootId = id;
        }

        #endregion
    }
}