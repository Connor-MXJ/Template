
using System.Data.Entity.ModelConfiguration;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.Domain.Context.Mapping
{
    public class ProductCategoryMap : EntityTypeConfiguration<ProductCategory>
    {
        public ProductCategoryMap()
        {
            ToTable("ProductCategories");
            HasKey(u => u.CategoryId);
            Property(u => u.CategoryNumber).IsRequired();
            Property(u => u.CategoryName).HasMaxLength(50).IsRequired(); 
        } 
    }

    public class TempGoodsMap : EntityTypeConfiguration<TempGoods>
    {
        public TempGoodsMap()
        {
            ToTable("TempGoods");
            HasKey(u => u.TempGoodsId);
            HasRequired(t => t.Product).WithMany(t => t.TempGoods).HasForeignKey(t => t.ProductId);
        }
    }

    public class ProductSelectedAttibutesMap : EntityTypeConfiguration<ProductSelectedAttibutes>
    { 
        public ProductSelectedAttibutesMap()
        {
            ToTable("ProductSelectedAttibutes");
            HasKey(u => u.ProductToAttibutesId);
            HasRequired(t => t.Product).WithMany(t => t.ProductSelectedAttibutes).HasForeignKey(t => t.ProductId);
        }
    }
}
