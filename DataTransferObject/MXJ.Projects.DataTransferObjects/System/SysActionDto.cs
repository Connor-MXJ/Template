using MXJ.Projects.Models.DataTransferObjects;
using System;

namespace MXJ.Projects.DataTransferObjects
{
    public class SysActionDto : BaseEntityDto<Guid>
    {
        #region Public Properties

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerCode { get; set; }
        /// <summary>
        /// 行为名称
        /// </summary>
        public string ActionCode { get; set; }
        /// <summary>
        ///     标识
        /// </summary>
        public Guid ActionID { get; set; }

        /// <summary>
        /// 是否记录日志
        /// </summary>
        public bool IsRequireLog { get; set; }

        /// <summary>
        /// 关联的菜单ID
        /// </summary>
        public Guid MenuID { get; set; }

        #endregion
    }
}