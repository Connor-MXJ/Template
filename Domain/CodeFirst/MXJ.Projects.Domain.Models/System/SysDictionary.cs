using System;
using MXJ.Core.Domain.Models;
using MXJ.Projects.Domain.Enums;

namespace MXJ.Projects.Domain.Models
{
    /// <summary>
    ///     数据字典
    /// </summary>
    public class SysDictionary : BaseEntity<Guid>
    {
        #region Public Properties

        /// <summary>
        ///     字典编码
        /// </summary>
        public string DictionaryCode { get; set; }

        /// <summary>
        ///     数据字典ID
        /// </summary>
        public Guid DictionaryId { get; set; }

        /// <summary>
        ///     字典名称
        /// </summary>
        public string DictionaryName { get; set; }

        /// <summary>
        ///     父级
        /// </summary>
        public DictionaryParentCode DictionaryParent { get; set; }

        /// <summary>
        ///     是否作废,true作废
        /// </summary>
        public bool Canceled { get; set; }

        /// <summary>
        ///     资金计划项ID
        /// </summary>
        public Guid? FinFundPlanItemId { get; set; }

        #endregion
    }
}