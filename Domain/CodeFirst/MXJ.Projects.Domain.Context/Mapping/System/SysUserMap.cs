using System.Data.Entity.ModelConfiguration;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.Domain.Context.Mapping
{
    /// <summary>
    ///     系统用户实体关系映射
    /// </summary>
    public class SysUserMap : EntityTypeConfiguration<SysUser>
    {
        /// <summary>
        ///     系统用户实体关系映射
        /// </summary>
        public SysUserMap()
        {
            ToTable("SysUser");
            HasKey(u => u.SysUserId); 
        }
    }
}