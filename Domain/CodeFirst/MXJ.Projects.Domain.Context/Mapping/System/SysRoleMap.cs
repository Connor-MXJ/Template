using System.Data.Entity.ModelConfiguration;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.Domain.Context.Mapping
{
    /// <summary>
    ///     角色实体关系映射
    /// </summary>
    public class SysRoleMap : EntityTypeConfiguration<SysRole>
    {
        /// <summary>
        ///     角色实体关系映射
        /// </summary>
        public SysRoleMap()
        {
            ToTable("SysRole");
            HasKey(u => u.SysRoleId);
            Property(u => u.RoleName).HasMaxLength(50).IsRequired();
            Property(u => u.RoleDesc).HasMaxLength(200);
        }
    }
}