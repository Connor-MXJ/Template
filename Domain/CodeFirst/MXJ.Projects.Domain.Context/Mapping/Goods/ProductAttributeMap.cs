
using System.Data.Entity.ModelConfiguration;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.Domain.Context.Mapping
{
    public class ProductAttributeMap : EntityTypeConfiguration<ProductAttribute>
    {
        public ProductAttributeMap()
        {
            ToTable("ProductAttributes");
            HasKey(u => u.AttributeId);
            Property(u => u.AttributeNumber).IsRequired();
            Property(u => u.AttributeName).HasMaxLength(50).IsRequired();
        }
    }

    public class AttributeValuesMap : EntityTypeConfiguration<AttributeValues>
    {
        public AttributeValuesMap()
        {
            ToTable("AttributeValues");
            HasKey(u => u.AttributeValueId);
            Property(u => u.AttributeValueNumber).IsRequired();
            Property(u => u.AttributeValue).HasMaxLength(50).IsRequired();
            HasRequired(t => t.ProductAttribute).WithMany(t => t.Values).HasForeignKey(t => t.ProductAttributeId);
        }
    }
}
