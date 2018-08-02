using System.Data.Entity.ModelConfiguration;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.Domain.Context.Mapping
{
    /// <summary>
    ///     菜单实体关系映射
    /// </summary>
    public class SysMenuMap : EntityTypeConfiguration<SysMenu>
    {
        /// <summary>
        ///     菜单实体关系映射
        /// </summary>
        public SysMenuMap()
        {
            ToTable("SysMenu");
            HasKey(u => u.SysMenuId);
            Property(u => u.MenuName).HasMaxLength(50).IsRequired();
        }
    }
}