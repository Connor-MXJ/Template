namespace MXJ.Projects.Domain.Context.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Area",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        CityCode = c.Int(nullable: false),
                        Name = c.String(unicode: false),
                        GpsLon = c.Decimal(precision: 18, scale: 2),
                        GpsLat = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.AttributeValues",
                c => new
                    {
                        AttributeValueId = c.Guid(nullable: false),
                        AttributeValue = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        ProductAttributeId = c.Guid(nullable: false),
                        AttributeValueNumber = c.Int(nullable: false),
                        RowVersion = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 0),
                        CreatedUser = c.Guid(),
                        UpdatedTime = c.DateTime(precision: 0),
                        UpdatedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.AttributeValueId)
                .ForeignKey("dbo.ProductAttributes", t => t.ProductAttributeId, cascadeDelete: true)
                .Index(t => t.ProductAttributeId);
            
            CreateTable(
                "dbo.ProductAttributes",
                c => new
                    {
                        AttributeId = c.Guid(nullable: false),
                        AttributeName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        IsAllowFilter = c.Boolean(nullable: false),
                        IsSaleAttribute = c.Boolean(nullable: false),
                        AttributeNumber = c.Int(nullable: false),
                        RowVersion = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 0),
                        CreatedUser = c.Guid(),
                        UpdatedTime = c.DateTime(precision: 0),
                        UpdatedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.AttributeId);
            
            CreateTable(
                "dbo.ProductCategories",
                c => new
                    {
                        CategoryId = c.Guid(nullable: false),
                        CategoryName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        IsOurSale = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        CategoryNumber = c.Int(nullable: false),
                        ProductPath = c.String(unicode: false),
                        ParentCategoryId = c.Guid(),
                        CategoryLevel = c.Int(nullable: false),
                        Url = c.String(unicode: false),
                        Title = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        Keywords = c.String(unicode: false),
                        RowVersion = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 0),
                        CreatedUser = c.Guid(),
                        UpdatedTime = c.DateTime(precision: 0),
                        UpdatedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        ProvinCode = c.Int(nullable: false),
                        Name = c.String(unicode: false),
                        GpsLon = c.Decimal(precision: 18, scale: 2),
                        GpsLat = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.DeliveryInfo",
                c => new
                    {
                        DeliveryInfoId = c.Guid(nullable: false),
                        ProvinceCode = c.Int(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        ProvinceName = c.String(unicode: false),
                        FirstWeight = c.Single(nullable: false),
                        FirstFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ContinueWeight = c.Single(nullable: false),
                        ContinueFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.DeliveryInfoId);
            
            CreateTable(
                "dbo.Goods",
                c => new
                    {
                        GoodsId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        AlreadySalesCount = c.Int(nullable: false),
                        ScaningCount = c.Int(nullable: false),
                        GoodsNumber = c.Long(nullable: false),
                        MinPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sku = c.String(unicode: false),
                        SystemSku = c.String(unicode: false),
                        SkuStandard = c.String(unicode: false),
                        LinkStr = c.String(unicode: false),
                        ProductProperty_ProductName = c.String(unicode: false),
                        ProductProperty_ProductNumber = c.String(unicode: false),
                        ProductProperty_ProductCategoryId = c.Guid(nullable: false),
                        ProductProperty_SupplierId = c.Guid(nullable: false),
                        ProductProperty_SupplierNumber = c.String(unicode: false),
                        ProductProperty_SupplierName = c.String(unicode: false),
                        ProductProperty_ProductModelType = c.String(unicode: false),
                        ProductProperty_ProductStatus = c.Int(nullable: false),
                        ProductProperty_SeoKeyWords = c.String(unicode: false),
                        ProductProperty_SeoDesc = c.String(unicode: false),
                        ProductProperty_MeansureUnit = c.Int(nullable: false),
                        ProductProperty_DeliveryCycle = c.String(unicode: false),
                        ProductProperty_Weight = c.Single(nullable: false),
                        ProductProperty_Volume = c.Single(nullable: false),
                        ProductProperty_ServiceAssurance = c.String(unicode: false),
                        ProductProperty_Spec = c.String(unicode: false),
                        ProductProperty_Desc = c.String(unicode: false),
                        ProductProperty_LogisticsValuation = c.Int(nullable: false),
                        ProductProperty_DeliveryDesc = c.String(unicode: false),
                        ProductProperty_NoPassReason = c.String(unicode: false),
                        RowVersion = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 0),
                        CreatedUser = c.Guid(),
                        UpdatedTime = c.DateTime(precision: 0),
                        UpdatedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.GoodsId);
            
            CreateTable(
                "dbo.GoodsAttributeValues",
                c => new
                    {
                        GoodsAttributeValueId = c.Guid(nullable: false),
                        ProductAttributeId = c.Guid(nullable: false),
                        AttributeValueId = c.Guid(nullable: false),
                        ProductAttributeNumber = c.Int(nullable: false),
                        AttributeValueNumber = c.Int(nullable: false),
                        Goods_GoodsId = c.Guid(),
                        TempGoods_TempGoodsId = c.Guid(),
                    })
                .PrimaryKey(t => t.GoodsAttributeValueId)
                .ForeignKey("dbo.Goods", t => t.Goods_GoodsId)
                .ForeignKey("dbo.TempGoods", t => t.TempGoods_TempGoodsId)
                .Index(t => t.Goods_GoodsId)
                .Index(t => t.TempGoods_TempGoodsId);
            
            CreateTable(
                "dbo.GoodsPicture",
                c => new
                    {
                        GoodsPictureId = c.Guid(nullable: false),
                        ServerUrl = c.String(unicode: false),
                        SmallFilePath = c.String(unicode: false),
                        FilePath = c.String(unicode: false),
                        IsPrimary = c.Boolean(nullable: false),
                        Goods_GoodsId = c.Guid(),
                        TempGoods_TempGoodsId = c.Guid(),
                    })
                .PrimaryKey(t => t.GoodsPictureId)
                .ForeignKey("dbo.Goods", t => t.Goods_GoodsId)
                .ForeignKey("dbo.TempGoods", t => t.TempGoods_TempGoodsId)
                .Index(t => t.Goods_GoodsId)
                .Index(t => t.TempGoods_TempGoodsId);
            
            CreateTable(
                "dbo.GoodsPrices",
                c => new
                    {
                        GoodsPriceId = c.Guid(nullable: false),
                        StartNumber = c.Int(nullable: false),
                        EndNumber = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Goods_GoodsId = c.Guid(),
                        Product_ProductId = c.Guid(),
                        TempGoods_TempGoodsId = c.Guid(),
                    })
                .PrimaryKey(t => t.GoodsPriceId)
                .ForeignKey("dbo.Goods", t => t.Goods_GoodsId)
                .ForeignKey("dbo.Products", t => t.Product_ProductId)
                .ForeignKey("dbo.TempGoods", t => t.TempGoods_TempGoodsId)
                .Index(t => t.Goods_GoodsId)
                .Index(t => t.Product_ProductId)
                .Index(t => t.TempGoods_TempGoodsId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductId = c.Guid(nullable: false),
                        ProductProperty_ProductName = c.String(unicode: false),
                        ProductProperty_ProductNumber = c.String(unicode: false),
                        ProductProperty_ProductCategoryId = c.Guid(nullable: false),
                        ProductProperty_SupplierId = c.Guid(nullable: false),
                        ProductProperty_SupplierNumber = c.String(unicode: false),
                        ProductProperty_SupplierName = c.String(unicode: false),
                        ProductProperty_ProductModelType = c.String(unicode: false),
                        ProductProperty_ProductStatus = c.Int(nullable: false),
                        ProductProperty_SeoKeyWords = c.String(unicode: false),
                        ProductProperty_SeoDesc = c.String(unicode: false),
                        ProductProperty_MeansureUnit = c.Int(nullable: false),
                        ProductProperty_DeliveryCycle = c.String(unicode: false),
                        ProductProperty_Weight = c.Single(nullable: false),
                        ProductProperty_Volume = c.Single(nullable: false),
                        ProductProperty_ServiceAssurance = c.String(unicode: false),
                        ProductProperty_Spec = c.String(unicode: false),
                        ProductProperty_Desc = c.String(unicode: false),
                        ProductProperty_LogisticsValuation = c.Int(nullable: false),
                        ProductProperty_DeliveryDesc = c.String(unicode: false),
                        ProductProperty_NoPassReason = c.String(unicode: false),
                        SubmitedTime = c.DateTime(precision: 0),
                        SubmitedUser = c.Guid(),
                        RowVersion = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 0),
                        CreatedUser = c.Guid(),
                        UpdatedTime = c.DateTime(precision: 0),
                        UpdatedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.ProductId);
            
            CreateTable(
                "dbo.ProductSelectedAttibutes",
                c => new
                    {
                        ProductToAttibutesId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        ProductAttributeId = c.Guid(nullable: false),
                        ProductAttributeValueId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ProductToAttibutesId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.TempGoods",
                c => new
                    {
                        TempGoodsId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        Sku = c.String(unicode: false),
                        SystemSku = c.String(unicode: false),
                        SkuStandard = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.TempGoodsId)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Province",
                c => new
                    {
                        Code = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        FullName = c.String(unicode: false),
                        GpsLon = c.Decimal(precision: 18, scale: 2),
                        GpsLat = c.Decimal(precision: 18, scale: 2),
                        CNPY = c.String(unicode: false),
                        CNPinyin = c.String(unicode: false),
                        SEOTitle = c.String(unicode: false),
                        SEOKeywords = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.SysAction",
                c => new
                    {
                        ActionID = c.Guid(nullable: false),
                        Name = c.String(unicode: false),
                        ControllerCode = c.String(unicode: false),
                        ActionCode = c.String(unicode: false),
                        IsRequireLog = c.Boolean(nullable: false),
                        MenuID = c.Guid(nullable: false),
                        RowVersion = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 0),
                        CreatedUser = c.Guid(),
                        UpdatedTime = c.DateTime(precision: 0),
                        UpdatedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.ActionID);
            
            CreateTable(
                "dbo.SysDictionary",
                c => new
                    {
                        DictionaryId = c.Guid(nullable: false),
                        DictionaryCode = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        DictionaryName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        DictionaryParent = c.Int(nullable: false),
                        Canceled = c.Boolean(nullable: false),
                        FinFundPlanItemId = c.Guid(),
                        RowVersion = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 0),
                        CreatedUser = c.Guid(),
                        UpdatedTime = c.DateTime(precision: 0),
                        UpdatedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.DictionaryId);
            
            CreateTable(
                "dbo.SysMenu",
                c => new
                    {
                        SysMenuId = c.Guid(nullable: false),
                        MenuName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        MenuOrder = c.Int(nullable: false),
                        MenuUrl = c.String(unicode: false),
                        MenuStyleName = c.String(unicode: false),
                        ParentId = c.Guid(),
                        RowVersion = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 0),
                        CreatedUser = c.Guid(),
                        UpdatedTime = c.DateTime(precision: 0),
                        UpdatedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.SysMenuId);
            
            CreateTable(
                "dbo.SysRole",
                c => new
                    {
                        SysRoleId = c.Guid(nullable: false),
                        IsSystemRole = c.Boolean(nullable: false),
                        RoleDesc = c.String(maxLength: 200, storeType: "nvarchar"),
                        RoleName = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        SysMenuIDs = c.String(unicode: false),
                        SysActionIDs = c.String(unicode: false),
                        RowVersion = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 0),
                        CreatedUser = c.Guid(),
                        UpdatedTime = c.DateTime(precision: 0),
                        UpdatedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.SysRoleId);
            
            CreateTable(
                "dbo.SysUser",
                c => new
                    {
                        SysUserId = c.Guid(nullable: false),
                        UserName = c.String(unicode: false),
                        UserPassword = c.String(unicode: false),
                        UserStatus = c.Int(nullable: false),
                        RealName = c.String(unicode: false),
                        RowVersion = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 0),
                        CreatedUser = c.Guid(),
                        UpdatedTime = c.DateTime(precision: 0),
                        UpdatedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.SysUserId);
            
            CreateTable(
                "dbo.UserRoleRef",
                c => new
                    {
                        RefId = c.Guid(nullable: false),
                        SysRoleId = c.Guid(nullable: false),
                        SysUserId = c.Guid(nullable: false),
                        RowVersion = c.Binary(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedTime = c.DateTime(nullable: false, precision: 0),
                        CreatedUser = c.Guid(),
                        UpdatedTime = c.DateTime(precision: 0),
                        UpdatedUser = c.Guid(),
                    })
                .PrimaryKey(t => t.RefId);
            
            CreateTable(
                "dbo.ProductCategoryProductAttributes",
                c => new
                    {
                        ProductCategory_CategoryId = c.Guid(nullable: false),
                        ProductAttribute_AttributeId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductCategory_CategoryId, t.ProductAttribute_AttributeId })
                .ForeignKey("dbo.ProductCategories", t => t.ProductCategory_CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.ProductAttributes", t => t.ProductAttribute_AttributeId, cascadeDelete: true)
                .Index(t => t.ProductCategory_CategoryId)
                .Index(t => t.ProductAttribute_AttributeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoodsAttributeValues", "TempGoods_TempGoodsId", "dbo.TempGoods");
            DropForeignKey("dbo.TempGoods", "ProductId", "dbo.Products");
            DropForeignKey("dbo.GoodsPrices", "TempGoods_TempGoodsId", "dbo.TempGoods");
            DropForeignKey("dbo.GoodsPicture", "TempGoods_TempGoodsId", "dbo.TempGoods");
            DropForeignKey("dbo.ProductSelectedAttibutes", "ProductId", "dbo.Products");
            DropForeignKey("dbo.GoodsPrices", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.GoodsPrices", "Goods_GoodsId", "dbo.Goods");
            DropForeignKey("dbo.GoodsPicture", "Goods_GoodsId", "dbo.Goods");
            DropForeignKey("dbo.GoodsAttributeValues", "Goods_GoodsId", "dbo.Goods");
            DropForeignKey("dbo.AttributeValues", "ProductAttributeId", "dbo.ProductAttributes");
            DropForeignKey("dbo.ProductCategoryProductAttributes", "ProductAttribute_AttributeId", "dbo.ProductAttributes");
            DropForeignKey("dbo.ProductCategoryProductAttributes", "ProductCategory_CategoryId", "dbo.ProductCategories");
            DropIndex("dbo.ProductCategoryProductAttributes", new[] { "ProductAttribute_AttributeId" });
            DropIndex("dbo.ProductCategoryProductAttributes", new[] { "ProductCategory_CategoryId" });
            DropIndex("dbo.TempGoods", new[] { "ProductId" });
            DropIndex("dbo.ProductSelectedAttibutes", new[] { "ProductId" });
            DropIndex("dbo.GoodsPrices", new[] { "TempGoods_TempGoodsId" });
            DropIndex("dbo.GoodsPrices", new[] { "Product_ProductId" });
            DropIndex("dbo.GoodsPrices", new[] { "Goods_GoodsId" });
            DropIndex("dbo.GoodsPicture", new[] { "TempGoods_TempGoodsId" });
            DropIndex("dbo.GoodsPicture", new[] { "Goods_GoodsId" });
            DropIndex("dbo.GoodsAttributeValues", new[] { "TempGoods_TempGoodsId" });
            DropIndex("dbo.GoodsAttributeValues", new[] { "Goods_GoodsId" });
            DropIndex("dbo.AttributeValues", new[] { "ProductAttributeId" });
            DropTable("dbo.ProductCategoryProductAttributes");
            DropTable("dbo.UserRoleRef");
            DropTable("dbo.SysUser");
            DropTable("dbo.SysRole");
            DropTable("dbo.SysMenu");
            DropTable("dbo.SysDictionary");
            DropTable("dbo.SysAction");
            DropTable("dbo.Province");
            DropTable("dbo.TempGoods");
            DropTable("dbo.ProductSelectedAttibutes");
            DropTable("dbo.Products");
            DropTable("dbo.GoodsPrices");
            DropTable("dbo.GoodsPicture");
            DropTable("dbo.GoodsAttributeValues");
            DropTable("dbo.Goods");
            DropTable("dbo.DeliveryInfo");
            DropTable("dbo.City");
            DropTable("dbo.ProductCategories");
            DropTable("dbo.ProductAttributes");
            DropTable("dbo.AttributeValues");
            DropTable("dbo.Area");
        }
    }
}
