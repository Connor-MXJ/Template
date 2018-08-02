
using System.Data.Entity.ModelConfiguration;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.Domain.Context.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("Products");
            HasKey(u => u.ProductId); 
        }
    }

    public class GoodsMap : EntityTypeConfiguration<Goods>
    {
        public GoodsMap()
        {
            ToTable("Goods");
            HasKey(u => u.GoodsId); 
        }
    }

    public class GoodsPictureMap : EntityTypeConfiguration<GoodsPicture>
    {
        public GoodsPictureMap()
        {
            ToTable("GoodsPicture");
            HasKey(u => u.GoodsPictureId);
        }
    }
    public class DeliveryInfoMap : EntityTypeConfiguration<DeliveryInfo>
    {
        public DeliveryInfoMap()
        {
            ToTable("DeliveryInfo");
            HasKey(u => u.DeliveryInfoId);
        }
    }
}
