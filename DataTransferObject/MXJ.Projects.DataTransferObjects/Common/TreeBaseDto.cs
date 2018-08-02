namespace MXJ.Projects.DataTransferObjects
{
    using global::System;

    using global::System.Runtime.Serialization;
    using MXJ.Projects.Models.DataTransferObjects;

    /// <summary>
    ///     树形控件DTO
    /// </summary>
    [Serializable]
    [DataContract]
    public class TreeBaseDto : BaseEntityDto<Guid>
    {
        #region Public Properties

        /// <summary>
        ///     图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        ///     id
        /// </summary>
        [DataMember]
        public string id { get; set; }

        /// <summary>
        ///     名称
        /// </summary>
        [DataMember]
        public string name { get; set; }

        /// <summary>
        ///     不能选中
        /// </summary>
        public bool nocheck { get; set; }

        /// <summary>
        ///     是否展开
        /// </summary>
        [DataMember]
        public bool open { get; set; }

        /// <summary>
        ///     父级ID
        /// </summary>
        [DataMember]
        public string pId { get; set; }

        /// <summary>
        ///     值
        /// </summary>
        public string value { get; set; }

        #endregion
    }
}