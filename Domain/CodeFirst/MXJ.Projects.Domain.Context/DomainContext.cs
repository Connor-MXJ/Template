using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using MXJ.Projects.Domain.Context.Mapping;
using MXJ.Projects.Domain.Models;
using Configuration = MXJ.Projects.Domain.Context.Migrations.Configuration;

namespace MXJ.Projects.Domain.Context
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]    //MYSQL配置
    public class DomainContext : DbContext
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        public DomainContext()
            : base("MySqlConnectionString")
        {
            Configuration.LazyLoadingEnabled = true;
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DomainContext, Configuration>());
        }

        /// <summary>
        ///     将加了密的密码解密
        /// </summary>
        /// <returns></returns>
        private static string DecryptConnectionString()
        {
            var builder =
                new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ToString());
            builder.Password = Core.Utility.Security.TripleDes.Decrypt(builder.Password);
            return builder.ConnectionString;
        }


        /// <summary>
        /// 创建规则
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region 通用
            modelBuilder.Configurations.Add(new ProvinceMap());
            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new AreaMap());
            #endregion

            #region 系统设置
            modelBuilder.Configurations.Add(new SysUserMap());
            modelBuilder.Configurations.Add(new SysMenuMap());
            modelBuilder.Configurations.Add(new SysDictionaryMap());
            modelBuilder.Configurations.Add(new SysRoleMap());
            modelBuilder.Configurations.Add(new SysActionMap());
            modelBuilder.Configurations.Add(new UserRoleRefMap());
            #endregion


            #region 商品 
            modelBuilder.Configurations.Add(new ProductCategoryMap());
            modelBuilder.Configurations.Add(new ProductAttributeMap());
            modelBuilder.Configurations.Add(new AttributeValuesMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new GoodsMap());
            modelBuilder.Configurations.Add(new TempGoodsMap());
            modelBuilder.Configurations.Add(new ProductSelectedAttibutesMap());
            modelBuilder.Configurations.Add(new DeliveryInfoMap());
            modelBuilder.Configurations.Add(new GoodsPictureMap());
            #endregion
                        
        }

        #region 通用

        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Area> Areas { get; set; }
        #endregion

        #region 系统设置 
        public DbSet<SysDictionary> SysDictionarys { get; set; }
        public DbSet<SysMenu> SysMenus { get; set; }
        public DbSet<SysRole> SysRoles { get; set; }
        public DbSet<SysUser> SysUsers { get; set; }

        public DbSet<SysAction> SysActions { get; set; }
        public DbSet<UserRoleRef> UserRoleRefs { get; set; }
        #endregion

        
        #region 商品 
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<AttributeValues> AttributeValues { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<TempGoods> TempGoods { get; set; }
        public DbSet<ProductSelectedAttibutes> ProductSelectedAttibutes { get; set; }
        public DbSet<DeliveryInfo> DeliveryInfo { get; set; }
        public DbSet<GoodsPicture> GoodsPictures { get; set; } 
        #endregion

        
    }
}