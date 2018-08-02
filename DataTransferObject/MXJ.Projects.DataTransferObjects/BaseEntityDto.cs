using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MXJ.Projects.Models.DataTransferObjects
{
    public class BaseEntityDto<TKey>
    {
        /// <summary>
        ///     并发控制时间戳
        /// </summary>
        public byte[] RowVersion { get; set; }

        /// <summary>
        ///     逻辑删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     创建时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        ///     创建者ID
        /// </summary>
        public Guid? CreatedUser { get; set; }

        /// <summary>
        ///     创建者Path
        /// </summary>
        public string CreatedUserPath { get; set; }

        /// <summary>
        ///     上次修改时间
        /// </summary>
        public DateTime? UpdatedTime { get; set; }

        /// <summary>
        ///     上次修改者ID
        /// </summary>
        public Guid? UpdatedUser { get; set; }
    }
}
