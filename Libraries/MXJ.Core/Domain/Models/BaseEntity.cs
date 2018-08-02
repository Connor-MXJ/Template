using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MXJ.Core.Domain.Models
{
    public class BaseEntity<TKey>
    {
        /// <summary>
        ///     并发控制时间戳
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
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
        ///     上次修改时间
        /// </summary>
        public DateTime? UpdatedTime { get; set; }

        /// <summary>
        ///     上次修改者ID
        /// </summary>
        public Guid? UpdatedUser { get; set; }
    }
}