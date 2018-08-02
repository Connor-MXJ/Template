namespace MXJ.Projects.DataTransferObjects
{
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Runtime.Serialization;
    public class ExcelResultBase
    {
        /// <summary>
        /// excel数据是否合法
        /// </summary>
        public bool IsExcelDataValid { get; set; }

        /// <summary>
        /// sheet 名称
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 标头
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// 标题集合
        /// </summary>
        public IList<string> TitleList { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public IList<IDictionary<string, object>> ExcelDataList { get; set; }

        /// <summary>
        /// 数据总行数
        /// </summary>
        public int TotalRows { get; set; }

        /// <summary>
        /// 错误行号
        /// </summary>
        public int ErrorRowNumber { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
