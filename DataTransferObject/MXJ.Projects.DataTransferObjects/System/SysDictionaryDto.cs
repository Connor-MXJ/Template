using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks; 
using MXJ.Projects.Domain.Enums;
using MXJ.Projects.Models.DataTransferObjects;
using MXJ.Core.Utility.Extensions;

namespace MXJ.Projects.DataTransferObjects
{
    [Serializable]
    [DataContract]
    public class SysDictionaryDto : BaseEntityDto<Guid>
    {
        #region Public Properties

        /// <summary>
        ///     字典编码
        /// </summary>
        [DataMember]
        public string DictionaryCode { get; set; }

        /// <summary>
        ///     数据字典ID
        /// </summary>
        [DataMember]
        public Guid DictionaryId { get; set; }

        /// <summary>
        ///     字典名称
        /// </summary>
        [DataMember]
        public string DictionaryName { get; set; }

        /// <summary>
        ///     父级
        /// </summary>
        [DataMember]
        public DictionaryParentCode DictionaryParent { get; set; }

        /// <summary>
        /// 是否作废,true作废
        /// </summary>
        public bool Canceled { get; set; }

        /// <summary>
        /// 是否作废,true作废
        /// </summary>
        public string DisplayCanceld { get { return Canceled.ToDisplayBool(); } }

        /// <summary>
        ///     父级显示值
        /// </summary>
        [DataMember]
        public string DisplayDictionaryParent { get { return DictionaryParent.ToString(); } }
         
        #endregion
    }

    #region SearchDTO

    /// <summary>
    ///     数据字典查询类
    /// </summary>
    [Serializable]
    [DataContract]
    public class SysDictionarySearchDto : SearchRequestBase
    {
        #region Public Properties

        /// <summary>
        ///     字典编码
        /// </summary>
        [DataMember]
        public string DictionaryCode { get; set; }

        /// <summary>
        ///     字典名称
        /// </summary>
        [DataMember]
        public string DictionaryName { get; set; }

        /// <summary>
        ///     父级
        /// </summary>
        [DataMember]
        public DictionaryParentCode DictionaryParent { get; set; }

        #endregion
    }

    #endregion
}
