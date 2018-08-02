using System.Data.Entity.ModelConfiguration;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.Domain.Context.Mapping
{
    /// <summary>
    ///     数据字典实体关系映射
    /// </summary>
    public class SysDictionaryMap : EntityTypeConfiguration<SysDictionary>
    {
        /// <summary>
        ///     数据字典实体关系映射
        /// </summary>
        public SysDictionaryMap()
        {
            ToTable("SysDictionary");
            HasKey(u => u.DictionaryId);
            Property(u => u.DictionaryCode).HasMaxLength(50).IsRequired();
            Property(u => u.DictionaryName).HasMaxLength(50).IsRequired();
            Property(u => u.DictionaryParent).IsRequired();
        }
    }
}