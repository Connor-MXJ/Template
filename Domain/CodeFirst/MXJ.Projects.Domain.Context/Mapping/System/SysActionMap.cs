using System.Data.Entity.ModelConfiguration;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.Domain.Context.Mapping
{
    /// <summary>
    ///     系统用户实体关系映射
    /// </summary>
    public class SysActionMap : EntityTypeConfiguration<SysAction>
    {
        /// <summary>
        ///     系统用户实体关系映射
        /// </summary>
        public SysActionMap()
        {
            ToTable("SysAction");
            HasKey(u => u.ActionID); 
        }
    }
}