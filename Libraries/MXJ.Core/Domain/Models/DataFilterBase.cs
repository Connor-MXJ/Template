using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Core.Domain.Models
{
    public class DataFilterBase:BaseEntity<Guid>
    {
        /// <summary>
        ///     所属组织
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        ///  地产公司
        /// </summary>
        public Guid LandId { get; set; }

        /// <summary>
        /// 地产公司名称
        /// </summary>
        public string LandName { get; set; }

        /// <summary>
        ///    所属项目
        /// </summary>
        public Guid? ProjectId { get; set; }

        /// <summary>
        ///    项目名称
        /// </summary>
        public string ProjectName { get; set; }
    }
}
