using System.Data.Entity.ModelConfiguration;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.Domain.Context.Mapping
{
    /// <summary>
    ///     系统用户实体关系映射
    /// </summary>
    public class UserRoleRefMap : EntityTypeConfiguration<UserRoleRef>
    {
        /// <summary>
        ///     系统用户实体关系映射
        /// </summary>
        public UserRoleRefMap()
        {
            ToTable("UserRoleRef");
            HasKey(u => u.RefId); 
        }
    }
}