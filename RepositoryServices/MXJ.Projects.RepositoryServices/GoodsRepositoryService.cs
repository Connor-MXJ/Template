using MySql.Data.MySqlClient;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq; 
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Domain.Context;
using MXJ.Projects.Domain.Models;
using MXJ.Projects.IRepositoryServices; 
using MXJ.Projects.Domain.Enums; 
using MXJ.Core;
using MXJ.Core.ValueInjector;

namespace MXJ.Projects.RepositoryServices
{
    /// <summary>
    ///     商品持久化层
    /// </summary>
    public class GoodsRepositoryService : BaseRepositoryService, IGoodsRepositoryService
    {
        public GoodsRepositoryService(IDomainRepositoryContext domainRepositoryContext,
            IDomainRepository<ProductCategory, Guid> productCategoryRepo,
            IDomainRepository<ProductAttribute, Guid> productAttributeRepo,
            IDomainRepository<Product, Guid> productRepo,
            IDomainRepository<AttributeValues, Guid> attributeValuesRepo)
        {
            DomainRepositoryContext = domainRepositoryContext;
            ProductCategoryRepo = productCategoryRepo;
            ProductAttributeRepo = productAttributeRepo;
            ProductRepo = productRepo;
            AttributeValuesRepo = attributeValuesRepo;
        }

        protected IDomainRepositoryContext DomainRepositoryContext { get; }
        protected IDomainRepository<ProductAttribute, Guid> ProductAttributeRepo { get; set; }
        protected IDomainRepository<ProductCategory, Guid> ProductCategoryRepo { get; set; }

        protected IDomainRepository<Product, Guid> ProductRepo { get; set; }

        protected IDomainRepository<AttributeValues, Guid> AttributeValuesRepo { get; set; }

        #region 商品分类

        /// <summary>
        ///     创建一个新的种类
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        public int CreateProductCategory(ProductCategoryDto productCategoryDto)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"INSERT INTO productcategories (CategoryId, CategoryName, IsOurSale, ProductPath,`Order`,
            ParentCategoryId,CategoryLevel,CreatedTime,CreatedUser,CategoryNumber) VALUES (@CategoryId, @CategoryName, 
            @IsOurSale, @ProductPath,@Order, @ParentCategoryId, @CategoryLevel,@CreatedTime,@CreatedUser,@CategoryNumber);";

            parameters.Add(new MySqlParameter("@CategoryId", productCategoryDto.CategoryId));
            parameters.Add(new MySqlParameter("@CategoryName", productCategoryDto.CategoryName));
            parameters.Add(new MySqlParameter("@IsOurSale", productCategoryDto.IsOurSale));
            parameters.Add(new MySqlParameter("@ProductPath", productCategoryDto.ProductPath));
            parameters.Add(new MySqlParameter("@Order", productCategoryDto.Order));
            parameters.Add(new MySqlParameter("@ParentCategoryId", productCategoryDto.ParentCategoryId));
            parameters.Add(new MySqlParameter("@CategoryLevel", productCategoryDto.CategoryLevel));
            parameters.Add(new MySqlParameter("@CreatedTime", productCategoryDto.CreatedTime));
            parameters.Add(new MySqlParameter("@CreatedUser", productCategoryDto.CreatedUser));
            parameters.Add(new MySqlParameter("@CategoryNumber", productCategoryDto.CategoryNumber));

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());

        }

        /// <summary>
        ///     修改一个种类
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        public int UpdateProductCategory(ProductCategoryDto productCategoryDto)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"Update productcategories set CategoryName=@CategoryName, IsOurSale=@IsOurSale, ProductPath=@ProductPath,`Order`=@Order,CategoryLevel=@CategoryLevel,UpdatedTime=@UpdatedTime,UpdatedUser=@UpdatedUser where CategoryId=@CategoryId;";

            parameters.Add(new MySqlParameter("@CategoryId", productCategoryDto.CategoryId));
            parameters.Add(new MySqlParameter("@CategoryName", productCategoryDto.CategoryName));
            parameters.Add(new MySqlParameter("@IsOurSale", productCategoryDto.IsOurSale));
            parameters.Add(new MySqlParameter("@ProductPath", productCategoryDto.ProductPath));
            parameters.Add(new MySqlParameter("@Order", productCategoryDto.Order));
            parameters.Add(new MySqlParameter("@CategoryLevel", productCategoryDto.CategoryLevel));
            parameters.Add(new MySqlParameter("@UpdatedTime", productCategoryDto.UpdatedTime));
            parameters.Add(new MySqlParameter("@UpdatedUser", productCategoryDto.UpdatedUser));
            try
            {
                return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取所有种类的树形结构树
        /// </summary>
        /// <returns></returns>
        public ResponseBase<List<ProductCategoryDto>> GetProductcategory()
        {
            ProductCategorySearchDto search = new ProductCategorySearchDto();
            var sql = BuildSqlGetProductCategories();
            var resource = ReadProductCategory(sql);
            ResponseBase<List<ProductCategoryDto>> response = new ResponseBase<List<ProductCategoryDto>>();
            response.Result = new List<ProductCategoryDto>();
            if (resource.Count > 0)
            {
                ProductCategoryDto dto;
                foreach (var item in resource.OrderBy(s => s.Order))
                {
                    dto = new ProductCategoryDto();
                    dto.id = item.CategoryId.ToString();
                    dto.pId = item.ParentCategoryId.ToString();
                    dto.name = item.CategoryName;
                    dto.CategoryLevel = item.CategoryLevel;
                    if (dto.CategoryLevel != 3) dto.open = true;
                    response.Result.Add(dto);
                }
            }
            return response;
        }

        /// <summary>
        /// 查询商品分类
        /// </summary> 
        /// <param name="search"></param> 
        /// <returns></returns>
        public List<ProductCategoryDto> GetProductCategories()
        {
            var sql = BuildSqlGetProductCategories();
            //using (MySqlConnection conn = new MySqlConnection(ConfigManager.DbConnectString))
            //{
            //    return conn.Query<ProductCategoryDto>(sql.Item1, null).ToList();
            //}

            return ReadProductCategory(sql);
        }

        /// <summary>
        /// 获取产品分类中最大的CategoryNumber
        /// </summary>
        /// <returns></returns>
        public int GetMaxCategoryNumberInProductCategory()
        {
            const string sql = @"select ifnull(max(CategoryNumber),0) max from `productcategories`;";

            try
            {
                var reader = MySqlHelper.ExecuteScalar(ConfigManager.DbConnectString, sql);
                int max = 0;
                int.TryParse(reader.ToString(), out max);
                return max;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 数据查询及对象映射
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private List<ProductCategoryDto> ReadProductCategory(Tuple<string, MySqlParameter[]> sql)
        {
            // 执行数据库查询
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql.Item1, sql.Item2);
            var list = new List<ProductCategoryDto>();
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new ProductCategoryDto
                    {
                        ProductPath = reader.GetValueOrDefault("ProductPath", string.Empty),
                        IsOurSale = reader.GetBoolean("IsOurSale"),
                        CategoryName = reader.GetValueOrDefault("CategoryName", string.Empty),
                        Order = reader.GetInt32("Order"),
                        CategoryId = reader.GetGuid("CategoryId"),
                        CategoryLevel = reader.GetInt32("CategoryLevel"),
                        CategoryNumber = reader.GetInt32("CategoryNumber"),
                        ParentCategoryId = reader.GetValueOrDefault<Guid?>("ParentCategoryId", null)
                    });
                }
            }

            return list;
        }

        /// <summary>
        ///  构造查询sql
        /// </summary>
        /// <param name="userName">登录账户</param>
        /// <param name="userPassword">密码</param>
        /// <param name="userStatus">状态</param>
        /// <param name="pageNumber">显示数量</param>
        /// <param name="pageIndex">页面索引（每页下标-1，mysql分页索引从0开始）</param>
        /// <returns></returns>
        private Tuple<string, MySqlParameter[]> BuildSqlGetProductCategories()
        {
            var parameters = new List<MySqlParameter>();
            string sql = "SELECT CategoryId,CategoryName,IsOurSale,CategoryNumber,ProductPath,`Order`,ParentCategoryId,CategoryLevel,CreatedTime,CreatedUser,UpdatedTime FROM productcategories where IsDeleted<> true Order by `Order` asc";

            return Tuple.Create(sql, parameters.ToArray());
        }

        #endregion

        #region 属性

        #region 属性
        /// <summary>
        ///     创建一个新的属性
        /// </summary>
        /// <param name="productAttributeDto"></param>
        /// <returns></returns>
        public int CreateProductAttribute(ProductAttributeDto productAttributeDto)
        {
            var parameters = new List<MySqlParameter>();
            string sql = @"INSERT INTO productattributes (AttributeId, AttributeName, IsAllowFilter, 
IsSaleAttribute,CreatedTime,CreatedUser,AttributeNumber) VALUES (@AttributeId, @AttributeName, @IsAllowFilter, 
@IsSaleAttribute,@CreatedTime,@CreatedUser,@AttributeNumber);";
            sql += @"insert into `productcategoryproductattributes` (ProductCategory_CategoryId,ProductAttribute_AttributeId) values 
                (@CategoryId,@AttributeId);";
            parameters.Add(new MySqlParameter("@AttributeId", productAttributeDto.AttributeId));
            parameters.Add(new MySqlParameter("@AttributeName", productAttributeDto.AttributeName));
            parameters.Add(new MySqlParameter("@IsAllowFilter", productAttributeDto.IsAllowFilter));
            parameters.Add(new MySqlParameter("@IsSaleAttribute", productAttributeDto.IsSaleAttribute));
            parameters.Add(new MySqlParameter("@CreatedTime", productAttributeDto.CreatedTime));
            parameters.Add(new MySqlParameter("@CreatedUser", productAttributeDto.CreatedUser));
            parameters.Add(new MySqlParameter("@CategoryId", productAttributeDto.ProductCategoryId));
            parameters.Add(new MySqlParameter("@AttributeNumber", productAttributeDto.AttributeNumber));

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        ///     编辑属性
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int EditProductAttribute(ProductAttributeDto dto)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"Update `productattributes` set AttributeName=@AttributeName, 
                                IsSaleAttribute=@IsSaleAttribute, IsAllowFilter=@IsAllowFilter,
                                UpdatedTime=@UpdatedTime,UpdatedUser=@UpdatedUser where AttributeId=@AttributeId";

            parameters.Add(new MySqlParameter("@AttributeId", dto.AttributeId));
            parameters.Add(new MySqlParameter("@AttributeName", dto.AttributeName));
            parameters.Add(new MySqlParameter("@IsSaleAttribute", dto.IsSaleAttribute));
            parameters.Add(new MySqlParameter("@IsAllowFilter", dto.IsAllowFilter));
            parameters.Add(new MySqlParameter("@UpdatedUser", dto.UpdatedUser));
            parameters.Add(new MySqlParameter("@UpdatedTime", dto.UpdatedTime));
            try
            {
                return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        ///     逻辑删除属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteProductAttribute(Guid id)
        {
            var parameters = new List<MySqlParameter>();
            //删除属性
            string sql = @"Update `productattributes` set IsDeleted=true where AttributeId=@AttributeId";
            //删除属性下的对应属性值
            sql = string.Format("{0};Update `attributevalues` set IsDeleted=true where ProductAttributeId=@AttributeId;", sql);

            parameters.Add(new MySqlParameter("@AttributeId", id));
            try
            {
                return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <returns></returns>
        public ProductAttributeDto GetProductAttributeByID(Guid attributeId)
        {
            var model = ProductAttributeRepo.GetByKey(attributeId);
            if (model != null)
            {
                ProductAttributeDto dto = new ProductAttributeDto();
                dto.Inject(model);
                dto.AttributeValues = new List<AttributeValuesDto>();

                var list = AttributeValuesRepo.Entities.Where(a => a.ProductAttributeId == attributeId);

                AttributeValuesDto attDto;
                foreach (var item in list)
                {
                    attDto = new AttributeValuesDto();
                    attDto.Inject(item);
                    dto.AttributeValues.Add(attDto);
                }

                return dto;
            }

            return null;
        }

        /// <summary>
        /// 获取属性相关信息
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns></returns>
        public List<ProductAttributeDto> GetProductAttributeByCategory(Guid categoryId)
        {
            var response = new List<ProductAttributeDto>();
            var parameters = new List<MySqlParameter>();

            #region 创建sql 
            string sql = @"select 
                               a.AttributeId,
                               a.AttributeName,
                               a.IsSaleAttribute,
                               a.IsAllowFilter,
                               a.AttributeNumber,
                               b.ProductCategory_CategoryId CategoryId,
                               ifnull(c.AttributeValue,'') AttributeValue,
                               c.AttributeValueId AttributeValueId,
							   c.AttributeValueNumber AttributeValueNumber
                        from (select * from `productcategoryproductattributes`  ) b
                        join  `productattributes` a
                        on 
                           a.attributeId = b.ProductAttribute_AttributeId
                        left join (select * from `attributevalues` where IsDeleted <> true) c
                        on 
                           c.ProductAttributeId = a.AttributeId
                        where 
                              a.IsDeleted <> true and b.ProductCategory_CategoryId=@CategoryId";

            parameters.Add(new MySqlParameter("@CategoryId", categoryId));

            #endregion

            #region 执行sql 

            var mySql = Tuple.Create(sql, parameters.ToArray());

            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, mySql.Item1, mySql.Item2);

            #endregion

            #region 绑定数据
            var list = new List<ProductAttributeDto>();
            using (reader)
            {
                Guid id;
                ProductAttributeDto dto;
                while (reader.Read())
                {
                    id = reader.GetGuid("AttributeId");
                    dto = list.Find(s => s.AttributeId == id);
                    if (dto == null)
                    {
                        dto = new ProductAttributeDto();
                        dto.AttributeId = reader.GetGuid("AttributeId");
                        dto.AttributeName = reader.GetValueOrDefault("AttributeName", string.Empty);
                        dto.IsAllowFilter = reader.GetBoolean("IsAllowFilter");
                        dto.IsSaleAttribute = reader.GetBoolean("IsSaleAttribute");
                        dto.AttributeNumber = reader.GetInt32("AttributeNumber");
                        dto.ProductCategoryId = categoryId;
                        if (!string.IsNullOrEmpty(reader.GetValueOrDefault("AttributeValue", string.Empty)))
                        {
                            dto.AttributeValues = new List<AttributeValuesDto>
                            {
                                new AttributeValuesDto
                                {
                                    AttributeValue =reader.GetValueOrDefault("AttributeValue",string.Empty) ,
                                    AttributeValueId =reader.GetGuid("AttributeValueId"),
                                    ProductAttributeId =  reader.GetGuid("AttributeId"),
                                    AttributeValueNumber = reader.GetInt32("AttributeValueNumber")
                                }
                            };
                        }
                    }
                    else
                    {
                        list.Remove(dto);
                        dto.AttributeValues.Add(new AttributeValuesDto
                        {
                            AttributeValue = reader.GetValueOrDefault("AttributeValue", string.Empty),
                            AttributeValueId = reader.GetGuid("AttributeValueId"),
                            ProductAttributeId = reader.GetGuid("AttributeId"),
                            AttributeValueNumber = reader.GetInt32("AttributeValueNumber")
                        });
                    }
                    list.Add(dto);
                }

                response = list;
            }

            #endregion

            return response;
        }

        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        public SearchResponseBase<List<ProductAttributeDto>> SearchProductAttribute(ProductAttributeSearchDto searchDto)
        {
            var response = new SearchResponseBase<List<ProductAttributeDto>>();
            var parameters = new List<MySqlParameter>();

            #region 创建sql 
            string sql = @"select
                               a.AttributeId,
                               a.AttributeName,
                               a.IsSaleAttribute,
                               a.IsAllowFilter,
                               b.ProductCategory_CategoryId CategoryId,
                               ifnull(c.AttributeValue,'') AttributeValue
                        from (select * from `productcategoryproductattributes` 
                                where ProductCategory_CategoryId=@CategoryId limit @startNum,@PageSize) b
                        join  `productattributes` a
                        on 
                           a.attributeId = b.ProductAttribute_AttributeId
                        left join (select * from `attributevalues` where IsDeleted=false) c
                        on 
                           c.ProductAttributeId = a.AttributeId
                        where 
                              a.IsDeleted = false ";
            parameters.Add(new MySqlParameter("@CategoryId", searchDto.CategoryID.Value));
            parameters.Add(new MySqlParameter("@startNum", (searchDto.Page - 1) * searchDto.Rows));
            parameters.Add(new MySqlParameter("@PageSize", searchDto.Rows));

            #endregion
            try
            {
                #region 执行sql 

                var mySql = Tuple.Create(sql, parameters.ToArray());

                var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, mySql.Item1, mySql.Item2);

                #endregion

                #region 绑定数据
                var list = new List<ProductAttributeDto>();
                using (reader)
                {
                    Guid id;
                    ProductAttributeDto dto;
                    while (reader.Read())
                    {
                        id = reader.GetGuid("AttributeId");
                        dto = list.Find(s => s.AttributeId == id);
                        if (dto == null)
                        {
                            dto = new ProductAttributeDto();
                            dto.AttributeId = reader.GetGuid("AttributeId");
                            dto.AttributeName = reader.GetValueOrDefault("AttributeName", string.Empty);
                            dto.IsAllowFilter = reader.GetBoolean("IsAllowFilter");
                            dto.IsSaleAttribute = reader.GetBoolean("IsSaleAttribute");
                            dto.ProductCategoryId = reader.GetGuid("CategoryId");
                            if (!string.IsNullOrEmpty(reader.GetValueOrDefault("AttributeValue", string.Empty)))
                            {
                                dto.AttributeValues = new List<AttributeValuesDto>
                            {
                                new AttributeValuesDto { AttributeValue=reader.GetValueOrDefault("AttributeValue",string.Empty) }
                            };
                            }
                        }
                        else
                        {
                            list.Remove(dto);
                            dto.AttributeValues.Add(new AttributeValuesDto() { AttributeValue = reader.GetValueOrDefault("AttributeValue", string.Empty) });
                        }
                        list.Add(dto);
                    }
                    if (reader.NextResult() && reader.Read())
                    {
                        response.TotalRecordCount = list.Count;
                    }
                    response.Result = list;
                }
                #endregion
            }
            catch (Exception ex)
            {
                response.Result = new List<ProductAttributeDto>();
                response.TotalRecordCount = 0;
            }
            response.Rows = searchDto.Rows;
            response.Page = searchDto.Page;
            response.IsSuccess = true;
            return response;
        }

        /// <summary>
        /// 获取产品属性中最大的AttributeNumber
        /// </summary>
        /// <returns></returns>
        public int GetMaxAttributeNumberInProductAttribute()
        {
            const string sql = @"select ifnull(max(AttributeNumber),0) max from `productattributes`;";

            try
            {
                var reader = MySqlHelper.ExecuteScalar(ConfigManager.DbConnectString, sql);
                int max = 0;
                int.TryParse(reader.ToString(), out max);
                return max;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 校验属性是否存在
        /// </summary>
        /// <param name="attName"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public bool IsExistProductAttribute(string attName, Guid categoryId)
        {
            bool result = false;
            var parameters = new List<MySqlParameter>();

            string sql = @"select count(*) `count` from `productattributes` a 
                            join `productcategoryproductattributes` b
                            on  a.attributeId = b.ProductAttribute_AttributeId
                            where b.ProductCategory_CategoryId=@CategoryId and a.AttributeName=@AttributeName;";
            parameters.Add(new MySqlParameter("@CategoryId", categoryId));
            parameters.Add(new MySqlParameter("@AttributeName", attName));

            var mySql = Tuple.Create(sql, parameters.ToArray());
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, mySql.Item1, mySql.Item2);
            using (reader)
            {
                while (reader.Read())
                {
                    if (reader.GetInt32("count") > 0)
                    {
                        result = true;
                    }
                }
                return result;
            }
        }

        #endregion

        #region 属性值

        /// <summary>
        /// 获取属性值信息
        /// </summary>
        /// <param name="id">属性值ID</param>
        /// <returns></returns>
        public AttributeValuesDto GetAttributeValueByID(Guid id)
        {
            var model = AttributeValuesRepo.GetByKey(id);
            if (model != null)
            {
                AttributeValuesDto dto = new AttributeValuesDto();
                dto.Inject(model);
                return dto;
            }
            return null;
        }

        /// <summary>
        /// 获取产品属性值中最大的AttributeValueNumber
        /// </summary>
        /// <returns></returns>
        public int GetMaxAttributeValueNumberInAttributeValue()
        {
            const string sql = @"select ifnull(max(AttributeValueNumber),0) max from `attributevalues`;";

            try
            {
                var reader = MySqlHelper.ExecuteScalar(ConfigManager.DbConnectString, sql);
                int max = 0;
                int.TryParse(reader.ToString(), out max);
                return max;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        ///     创建一个新的属性值
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        public int CreateAttributeValue(AttributeValuesDto attributeValuesDto)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"INSERT INTO attributevalues (AttributeValueId, AttributeValue, ProductAttributeId, 
                CreatedTime,CreatedUser,AttributeValueNumber) VALUES (@AttributeValueId, @AttributeValue, @ProductAttributeId,
                @CreatedTime,@CreatedUser,@AttributeValueNumber)";

            parameters.Add(new MySqlParameter("@AttributeValueId", attributeValuesDto.AttributeValueId));
            parameters.Add(new MySqlParameter("@AttributeValue", attributeValuesDto.AttributeValue));
            parameters.Add(new MySqlParameter("@ProductAttributeId", attributeValuesDto.ProductAttributeId));
            parameters.Add(new MySqlParameter("@CreatedTime", attributeValuesDto.CreatedTime));
            parameters.Add(new MySqlParameter("@CreatedUser", attributeValuesDto.CreatedUser));
            parameters.Add(new MySqlParameter("@AttributeValueNumber", attributeValuesDto.AttributeValueNumber));
            try
            {
                return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        ///     编辑属性值
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public int EditProductAttributeValue(AttributeValuesDto dto)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"Update `attributevalues` set AttributeValue=@AttributeValue,
                                ProductAttributeId=@ProductAttributeId
                                UpdatedTime=@UpdatedTime,UpdatedUser=@UpdatedUser 
                                where AttributeValueId=@AttributeValueId;";

            parameters.Add(new MySqlParameter("@AttributeValueId", dto.AttributeValueId));
            parameters.Add(new MySqlParameter("@ProductAttributeId", dto.ProductAttributeId));
            parameters.Add(new MySqlParameter("@AttributeValue", dto.AttributeValue));
            parameters.Add(new MySqlParameter("@UpdatedUser", dto.UpdatedUser));
            parameters.Add(new MySqlParameter("@UpdatedTime", dto.UpdatedTime));
            try
            {
                return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        ///     逻辑删除属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteProductAttributeValue(Guid id)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"Update `attributevalues` set IsDeleted=true where AttributeValueId=@AttributeValueId;";

            parameters.Add(new MySqlParameter("@AttributeValueId", id));
            try
            {
                return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 校验属性值是否存在
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResponseBase<bool> IsExistAttributeValue(AttributeValuesDto dto)
        {
            var response = new ResponseBase<bool>();
            response.IsSuccess = true;
            response.Result = false;
            if (AttributeValuesRepo.Entities.Any(s =>
                            s.AttributeValueId != dto.AttributeValueId &&
                            s.AttributeValue == dto.AttributeValue &&
                            s.ProductAttributeId == dto.ProductAttributeId))
            {
                response.Result = true;
            }
            return response;
        }

        #endregion

        #endregion

        #region 商品
        /// <summary>
        ///     创建一个新的商品
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        public int CreateProdut(ProductDto productDto)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"INSERT INTO Products (ProductId, ProductProperty_ProductName, ProductProperty_ProductNumber, ProductProperty_ProductCategoryId,ProductProperty_SupplierId,
ProductProperty_SupplierNumber,ProductProperty_SupplierName,ProductProperty_ProductStatus,ProductProperty_DeliveryCycle,CreatedTime,CreatedUser,IsDeleted)
VALUES (@ProductId, @ProductName, @ProductNumber, @ProductCategoryId,@SupplierId,
@SupplierNumber,@SupplierName,@ProductStatus,@DeliveryCycle,@CreatedTime,@CreatedUser,@IsDeleted);";

            parameters.Add(new MySqlParameter("@ProductId", productDto.ProductId));
            parameters.Add(new MySqlParameter("@ProductName", productDto.ProductProperty.ProductName));
            parameters.Add(new MySqlParameter("@ProductNumber", productDto.ProductProperty.ProductNumber));
            parameters.Add(new MySqlParameter("@ProductCategoryId", productDto.ProductProperty.ProductCategoryId));
            parameters.Add(new MySqlParameter("@SupplierId", productDto.ProductProperty.SupplierId));
            parameters.Add(new MySqlParameter("@SupplierNumber", productDto.ProductProperty.SupplierNumber));
            parameters.Add(new MySqlParameter("@SupplierName", productDto.ProductProperty.SupplierName));
            parameters.Add(new MySqlParameter("@ProductStatus", productDto.ProductProperty.ProductStatus));
            parameters.Add(new MySqlParameter("@DeliveryCycle", productDto.ProductProperty.DeliveryCycle));
            parameters.Add(new MySqlParameter("@CreatedTime", productDto.CreatedTime));
            parameters.Add(new MySqlParameter("@CreatedUser", productDto.CreatedUser));
            parameters.Add(new MySqlParameter("@IsDeleted", productDto.IsDeleted));
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 更新产品第二步
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int UpdateProductStepTwo(ProductDto productDto)
        {
            var parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@ProductProperty_ProductName", productDto.ProductProperty.ProductName));
            parameters.Add(new MySqlParameter("@ProductProperty_DeliveryCycle", productDto.ProductProperty.DeliveryCycle));
            parameters.Add(new MySqlParameter("@ProductProperty_SupplierId", productDto.ProductProperty.SupplierId));
            parameters.Add(new MySqlParameter("@ProductProperty_SupplierName", productDto.ProductProperty.SupplierName));
            parameters.Add(new MySqlParameter("@ProductProperty_SupplierNumber", productDto.ProductProperty.SupplierNumber));
            return UpdateProduct(productDto.ProductId, parameters);
        }

        /// <summary>
        ///     创建一个新的商品选定属性
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        public int CreateProductSelectedAttibutes(ProductSelectedAttibutesDto dto)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"INSERT INTO ProductSelectedAttibutes (ProductToAttibutesId, ProductId, ProductAttributeId, ProductAttributeValueId)
VALUES (@ProductToAttibutesId, @ProductId, @ProductAttributeId, @ProductAttributeValueId);";
            parameters.Add(new MySqlParameter("@ProductToAttibutesId", dto.ProductToAttibutesId));
            parameters.Add(new MySqlParameter("@ProductId", dto.ProductId));
            parameters.Add(new MySqlParameter("@ProductAttributeId", dto.ProductAttributeId));
            parameters.Add(new MySqlParameter("@ProductAttributeValueId", dto.ProductAttributeValueId));

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 物理删除产品所选属性
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public int DeleteProductSelectedAttributevVlues(Guid ProductId)
        {
            var parameters = new List<MySqlParameter>();
            string sql = @"Delete from ProductSelectedAttibutes  where ProductId=@ProductId ";
            parameters.Add(new MySqlParameter("@ProductId", ProductId));
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 更新产品SetpFive
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int UpdateProductStepFive(ProductDto productDto)
        {
            var parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@ProductProperty_Desc", productDto.ProductProperty.Desc));
            parameters.Add(new MySqlParameter("@ProductProperty_Spec", productDto.ProductProperty.Spec));
            parameters.Add(new MySqlParameter("@ProductProperty_ServiceAssurance", productDto.ProductProperty.ServiceAssurance));
            return UpdateProduct(productDto.ProductId, parameters);
        }

        /// <summary>
        /// 更新产品规格
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int UpdateProductStandard(ProductDto productDto)
        {
            var parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@ProductProperty_ProductModelType", productDto.ProductProperty.ProductModelType));
            return UpdateProduct(productDto.ProductId, parameters);
        }

        /// <summary>
        /// 更新产品交易信息
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public int UpdateProductTradeInfo(ProductDto productDto)
        {
            var parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@ProductProperty_MeansureUnit", productDto.ProductProperty.MeansureUnit));
            parameters.Add(new MySqlParameter("@ProductProperty_LogisticsValuation", productDto.ProductProperty.LogisticsValuation));
            parameters.Add(new MySqlParameter("@ProductProperty_Weight", productDto.ProductProperty.Weight));
            parameters.Add(new MySqlParameter("@ProductProperty_Volume", productDto.ProductProperty.Volume));
            return UpdateProduct(productDto.ProductId, parameters);
        }

        /// <summary>
        /// 更新配送说明
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public int UpdateProductDeliveryDesc(ProductDto productDto)
        {
            var parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@ProductProperty_DeliveryDesc", productDto.ProductProperty.DeliveryDesc));
            return UpdateProduct(productDto.ProductId, parameters);
        }
        /// <summary>
        /// 更新产品未通过原因
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public int UpdateProductNoPassReason(Guid productId, string reason)
        {
            var parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@ProductProperty_NoPassReason", reason));
            return UpdateProduct(productId, parameters);
        }

        /// <summary>
        /// 更新产品状态字段
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public int UpdateProductStatus(Guid productId, ProductStatusCode ProductStatus)
        {
            var parameters = new List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@ProductProperty_ProductStatus", (int)ProductStatus));
            return UpdateProduct(productId, parameters);
        }

        /// <summary>
        /// 处理配送信息
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public int ProcessProductDelivery(ProductDto productDto)
        {
            string sql = string.Empty;
            foreach (var delivery in productDto.DeliveryInfo)
            {
                sql += @" insert into deliveryinfo (DeliveryInfoId,ProvinceCode,ProductId,ProvinceName,FirstWeight,FirstFee,ContinueWeight,ContinueFee) values('" + Guid.NewGuid() + "'," + delivery.ProvinceCode + ",'" + delivery.ProductId + "','" + delivery.ProvinceName + "'," + delivery.FirstWeight + "," + delivery.FirstFee + "," + delivery.ContinueWeight + "," + delivery.ContinueFee + ");";
            }

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql);
        }

        /// <summary>
        /// 获取Product配送信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<DeliveryInfoDto> GetDeliveryInfo(Guid productId)
        {
            var parameters = new List<MySqlParameter>();
            string sql = @"select * from deliveryinfo  where ProductId = @ProductId";
            parameters.Add(new MySqlParameter("@ProductId", productId));
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql, parameters.ToArray());
            var list = new List<DeliveryInfoDto>();
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new DeliveryInfoDto
                    {
                        DeliveryInfoId = reader.GetGuid("DeliveryInfoId"),
                        ProductId = reader.GetGuid("ProductId"),
                        ProvinceCode = reader.GetInt32("ProvinceCode"),
                        ProvinceName = reader.GetValueOrDefault("ProvinceName", string.Empty),
                        FirstWeight = reader.GetFloat("FirstWeight"),
                        FirstFee = reader.GetDecimal("FirstFee"),
                        ContinueWeight = reader.GetFloat("ContinueWeight"),
                        ContinueFee = reader.GetDecimal("ContinueFee"),
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 更新产品实现
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private int UpdateProduct(Guid productId, List<MySqlParameter> parameters)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update Products set ");
            foreach (var item in parameters)
            {
                sql.Append(item.ParameterName.Remove(0, 1) + "=" + item.ParameterName + ",");
            }
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" where ProductId =  @ProductId");
            parameters.Add(new MySqlParameter("@ProductId", productId));
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql.ToString(), parameters.ToArray());
        }

        /// <summary>
        /// 物理删除属性配置表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public int DeleteAttributevVlues(SelectedAttibutesDto search)
        {
            var parameters = new List<MySqlParameter>();
            string sql = @"Delete from goodsattributevalues  where 1=1 ";
            if (search.GoodsId.HasValue && search.GoodsId.Value != Guid.Empty)
            {
                sql += " and Goods_GoodsId = @GoodsId";
                parameters.Add(new MySqlParameter("@GoodsId", search.GoodsId.Value));
            }

            if (search.TempGoodsId.HasValue && search.TempGoodsId.Value != Guid.Empty)
            {
                sql += " and TempGoods_TempGoodsId = @TempGoodsId";
                parameters.Add(new MySqlParameter("@TempGoodsId", search.TempGoodsId.Value));
            }

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 获取属性配置表
        /// </summary>
        /// <param name="tempGoodsId"></param>
        /// <returns></returns>
        public List<SelectedAttibutesDto> GetAttributeValues(SelectedAttibutesDto search)
        {
            var parameters = new List<MySqlParameter>();
            string sql = @"select * from goodsattributevalues  where 1=1 ";

            if (search.GoodsId.HasValue && search.GoodsId.Value != Guid.Empty)
            {
                sql += " and Goods_GoodsId = @GoodsId";
                parameters.Add(new MySqlParameter("@GoodsId", search.GoodsId.Value));
            }

            if (search.TempGoodsId.HasValue && search.TempGoodsId.Value != Guid.Empty)
            {
                sql += " and TempGoods_TempGoodsId = @TempGoodsId";
                parameters.Add(new MySqlParameter("@TempGoodsId", search.TempGoodsId.Value));
            }

            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql, parameters.ToArray());
            var list = new List<SelectedAttibutesDto>();
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new SelectedAttibutesDto
                    {
                        ProductAttributeId = reader.GetGuid("ProductAttributeId"),
                        ProductAttributeValueId = reader.GetGuid("AttributeValueId"),
                        TempGoodsId = reader.GetValueOrDefault<Guid?>("TempGoods_TempGoodsId", null),
                        GoodsId = reader.GetValueOrDefault<Guid?>("Goods_GoodsId", null)
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 存储临时goods的销售属性及值
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        public int CreateTempGoodsAttributeValues(TempGoodsDto tempGoodsDto)
        {
            var sql = string.Empty;
            foreach (var attr in tempGoodsDto.SelectedAttibutesAndValues.Split('|'))
            {
                if (string.IsNullOrEmpty(attr) || attr.IndexOf(":") < 0)
                {
                    continue;
                }

                var value = attr.Split(':');

                sql += @"INSERT INTO goodsattributevalues (GoodsAttributeValueId, ProductAttributeId, AttributeValueId, TempGoods_TempGoodsId)
VALUES ('" + Guid.NewGuid() + "', '" + value[0] + "', '" + value[1] + "', '" + tempGoodsDto.TempGoodsId + "');";
            }

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql);
        }

        /// <summary>
        /// 存储goods的属性及值
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        public int CreateGoodsAttributeValues(GoodsDto goodsDto)
        {
            var sql = string.Empty;
            foreach (var attr in goodsDto.GoodsAttributeValues)
            {
                sql += @"INSERT INTO goodsattributevalues (GoodsAttributeValueId, ProductAttributeId, AttributeValueId, Goods_GoodsId,ProductAttributeNumber,AttributeValueNumber)
VALUES ('" + Guid.NewGuid() + "', '" + attr.AttributeId + "', '" + attr.AttributeValueId + "', '" + goodsDto.GoodsId + "'," + attr.AttributeNumber + "," + attr.AttributeValueNumber + ");";
            }

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql);
        }

        /// <summary>
        /// 存储一个临时Goods
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        public int CreateTempGoods(TempGoodsDto tempGoodsDto)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"INSERT INTO tempgoods (TempGoodsId, ProductId, Sku, SystemSku,SkuStandard)
VALUES (@TempGoodsId, @ProductId, @Sku, @SystemSku,@SkuStandard );";

            parameters.Add(new MySqlParameter("@TempGoodsId", tempGoodsDto.TempGoodsId));
            parameters.Add(new MySqlParameter("@ProductId", tempGoodsDto.ProductId));
            parameters.Add(new MySqlParameter("@Sku", tempGoodsDto.Sku));
            parameters.Add(new MySqlParameter("@SystemSku", tempGoodsDto.SystemSku));
            parameters.Add(new MySqlParameter("@SkuStandard", tempGoodsDto.SkuStandard));

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 物理删除TempGoods
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int DeleteTempGoodsByProductId(Guid productId)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"Delete from tempgoods where ProductId=@ProductId; ";
            parameters.Add(new MySqlParameter("@ProductId", productId));
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        public int CreatePictures(TempGoodsDto tempGoodsDto)
        {
            var sql = string.Empty;
            foreach (var item in tempGoodsDto.Pictures)
            {
                sql += @" INSERT INTO goodspicture (GoodsPictureId, ServerUrl, SmallFilePath, FilePath,IsPrimary,
TempGoods_TempGoodsId)
VALUES ('" + Guid.NewGuid() + "' , '" + item.ServerUrl + "', '" + item.SmallFilePath + "', '" + item.FilePath + "'," + item.IsPrimary + ",'" + tempGoodsDto.TempGoodsId + "') ;";
            }

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql);
        }

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="goodsDto"></param>
        /// <returns></returns>
        public int CreatePictures(GoodsDto goodsDto)
        {
            var sql = string.Empty;
            foreach (var item in goodsDto.Pictures)
            {
                sql += @" INSERT INTO goodspicture (GoodsPictureId, ServerUrl, SmallFilePath, FilePath,IsPrimary,
 Goods_GoodsId)
VALUES ('" + Guid.NewGuid() + "' , '" + item.ServerUrl + "', '" + item.SmallFilePath + "', '" + item.FilePath + "'," + item.IsPrimary + ",'" + goodsDto.GoodsId + "') ;";
            }

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql);
        }

        /// <summary>
        /// 创建TempGoods对应价格
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        public int CreateTempGoodsPrices(TempGoodsDto tempGoodsDto)
        {
            var sql = string.Empty;
            foreach (var item in tempGoodsDto.Prices)
            {
                sql += @" INSERT INTO goodsprices (GoodsPriceId, StartNumber, EndNumber, Price,
TempGoods_TempGoodsId)
VALUES ('" + Guid.NewGuid() + "' , '" + item.StartNumber + "', '" + item.EndNumber + "', '" + item.Price + "','" + tempGoodsDto.TempGoodsId + "'); ";
            }
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql);
        }

        /// <summary>
        /// 创建Goods对应价格
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        public int CreateGoodsPrices(GoodsDto goodsDto)
        {
            var sql = string.Empty;
            foreach (var item in goodsDto.Prices)
            {
                sql += @" INSERT INTO goodsprices (GoodsPriceId, StartNumber, EndNumber, Price,
Goods_GoodsId)
VALUES ('" + Guid.NewGuid() + "' , '" + item.StartNumber + "', '" + item.EndNumber + "', '" + item.Price + "','" + goodsDto.GoodsId + "'); ";
            }
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql);
        }

        /// <summary>
        /// 创建Product对应价格
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        public int CreatePricesProduct(ProductDto productDto)
        {
            var sql = string.Empty;
            foreach (var item in productDto.Prices)
            {
                sql += @" INSERT INTO goodsprices (GoodsPriceId, StartNumber, EndNumber, Price,
Product_ProductId)
VALUES ('" + Guid.NewGuid() + "' , '" + item.StartNumber + "', '" + item.EndNumber + "', '" + item.Price + "','" + productDto.ProductId + "'); ";
            }

            if (!string.IsNullOrEmpty(sql))
            {
                return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 删除多个商品
        /// </summary>
        /// <param name="ids">商品Id集合</param>
        /// <returns></returns>
        public int DeleteProductById(List<Guid> ids)
        {
            var parameters = new List<MySqlParameter>();
            string param = "(";
            foreach (var item in ids)
            {
                param += string.Format(@",'{0}'", item);
            }
            if (param.Length > 1)
                param = param.Replace("(,", "");
            string sql = string.Format(@"update `products` set IsDeleted=true where ProductId in ({0});", param);

            //parameters.Add(new MySqlParameter("@ProductId", id));

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql);
        }

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="ids">商品Id集合</param>
        /// <param name="initStatus">初始状态</param>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="submintUser">更新用户</param>
        /// <returns></returns>
        public int SetProductById(List<Guid> ids,
            ProductStatusCode? initStatus,
            ProductStatusCode targetStatus,
            Guid submintUser)
        {
            var parameters = new List<MySqlParameter>();
            string sql = "";
            //将草稿状态的产品更新为待审核状态
            foreach (var item in ids)
            {
                sql += string.Format(@"update `products` set 
                        ProductProperty_ProductStatus=@ProductStatus,
                        SubmitedTime = @SubmitedTime,
                        SubmitedUser = @SubmitedUser
                        where ProductId = '{0}'", item);
            }
            if (initStatus.HasValue && Enum.IsDefined(typeof(ProductStatusCode), initStatus.Value))
            {
                sql += " and ProductProperty_ProductStatus=@Status";
                parameters.Add(new MySqlParameter("@Status", initStatus));
            }
            parameters.Add(new MySqlParameter("@ProductStatus", targetStatus));
            parameters.Add(new MySqlParameter("@SubmitedTime", DateTime.Now));
            parameters.Add(new MySqlParameter("@SubmitedUser", submintUser));

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 更新Goods商品状态
        /// </summary>
        /// <param name="ids">Goods商品Id集合</param>
        /// <param name="initStatus">初始状态</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns></returns>
        public int SetGoodsStatusById(List<Guid> ids, ProductStatusCode? initStatus, ProductStatusCode targetStatus)
        {
            var parameters = new List<MySqlParameter>();
            string sql = "";
            foreach (var item in ids)
            {
                sql += string.Format(@"update `goods` set ProductProperty_ProductStatus=@ProductStatus 
                        where GoodsId = '{0}'", item);
            }
            if (initStatus.HasValue && Enum.IsDefined(typeof(ProductStatusCode), initStatus.Value))
            {
                sql += " and ProductProperty_ProductStatus=@Status";
                parameters.Add(new MySqlParameter("@Status", initStatus));
            }
            parameters.Add(new MySqlParameter("@ProductStatus", targetStatus));

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 获取商品个数
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public int GetProductNumberByFilter(ProductSearchDto search)
        {
            var parameters = new List<MySqlParameter>();
            string sql = @"select count(*) number from `products` where IsDeleted=false";

            if (search.Status != null)
            {
                string filter = "(";
                foreach (var item in search.Status)
                {
                    if (Enum.IsDefined(typeof(ProductStatusCode), item))
                    {
                        filter += string.Format(",{0}", (int)item);
                    }

                }
                if (filter.Length > 1)
                {
                    filter = filter.Replace("(,", "");
                    sql += string.Format(@" and ProductProperty_ProductStatus in ({0})", filter);
                }
            }

            if (Enum.IsDefined(typeof(LoginRoleCode), search.UserRole))
            {
                if (search.UserRole == LoginRoleCode.供应商 && search.UserId.HasValue)
                {
                    sql += " and ProductProperty_SupplierId=@User";
                    parameters.Add(new MySqlParameter("@User", search.UserId.Value));
                }
            }
            var reader = MySqlHelper.ExecuteScalar(ConfigManager.DbConnectString, sql, parameters.ToArray());
            int total = 0;
            int.TryParse(reader.ToString(), out total);
            return total;
        }

        /// <summary>
        /// 获取商品个数
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public int GetGoodsNumberByFilter(GoodsSearchDto search)
        {
            var parameters = new List<MySqlParameter>();
            string sql = @"select count(*) number from `goods` where IsDeleted=false";

            if (search.Status != null)
            {
                string filter = "(";
                foreach (var item in search.Status)
                {
                    if (Enum.IsDefined(typeof(ProductStatusCode), item))
                    {
                        filter += string.Format(",{0}", (int)item);
                    }

                }
                if (filter.Length > 1)
                {
                    filter = filter.Replace("(,", "");
                    sql += string.Format(@" and ProductProperty_ProductStatus in ({0})", filter);
                }
            }

            if (Enum.IsDefined(typeof(LoginRoleCode), search.UserRole))
            {
                if (search.UserRole == LoginRoleCode.供应商 && search.UserId.HasValue)
                {
                    sql += " and CreatedUser=@User";
                    parameters.Add(new MySqlParameter("@User", search.UserId.Value));
                }
            }
            var reader = MySqlHelper.ExecuteScalar(ConfigManager.DbConnectString, sql, parameters.ToArray());
            int total = 0;
            int.TryParse(reader.ToString(), out total);
            return total;
        }

        /// <summary>
        /// 物理删除Pictures by TempGoods
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int DeletePicturesByTempGoodsId(Guid tempGoodsId)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"Delete from goodspicture where TempGoods_TempGoodsId=@GoodsId ;";
            parameters.Add(new MySqlParameter("@GoodsId", tempGoodsId));
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }


        /// <summary>
        /// 物理删除Price by TempGoods
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int DeletePricesByTempGoodsId(Guid tempGoodsId)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"Delete from goodsprices where TempGoods_TempGoodsId=@GoodsId ";
            parameters.Add(new MySqlParameter("@GoodsId", tempGoodsId));
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<ProductDto> GetProducts(ProductSearchDto search, out int total)
        {
            var sql = BuildSqlGetProducts(search);
            return ReadProduct(sql, out total);
        }

        /// <summary>
        /// 数据查询及对象映射
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private List<ProductDto> ReadProduct(Tuple<string, MySqlParameter[]> sql, out int total)
        {
            // 执行数据库查询
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql.Item1, sql.Item2);
            var list = new List<ProductDto>();
            total = 0;
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new ProductDto
                    {
                        ProductId = reader.GetGuid("ProductId"),
                        IsDeleted = reader.GetBoolean("IsDeleted"),
                        CreatedTime = reader.GetDateTime("CreatedTime"),
                        CreatedUser = reader.GetValueOrDefault<Guid?>("CreatedUser", null),
                        UpdatedTime = reader.GetValueOrDefault<DateTime?>("UpdatedTime", null),
                        UpdatedUser = reader.GetValueOrDefault<Guid?>("UpdatedUser", null),
                        ProductProperty = new ProductPropertyDto
                        {
                            ProductName = reader.GetValueOrDefault("ProductProperty_ProductName", string.Empty),
                            ProductCategoryId = reader.GetGuid("ProductProperty_ProductCategoryId"),
                            SupplierId = reader.GetGuid("ProductProperty_SupplierId"),
                            SupplierNumber = reader.GetValueOrDefault("ProductProperty_SupplierNumber", string.Empty),
                            ProductModelType = reader.GetValueOrDefault("ProductProperty_ProductModelType", string.Empty),
                            ProductStatus = (ProductStatusCode)reader.GetInt32("ProductProperty_ProductStatus"),
                            SeoKeyWords = reader.GetValueOrDefault("ProductProperty_SeoKeyWords", string.Empty),
                            SeoDesc = reader.GetValueOrDefault("ProductProperty_SeoDesc", string.Empty),
                            MeansureUnit = reader.GetInt32("ProductProperty_MeansureUnit") != 0 ? (MeansureUnitCode)reader.GetInt32("ProductProperty_MeansureUnit") : MeansureUnitCode.个,
                            DeliveryCycle = reader.GetValueOrDefault("ProductProperty_DeliveryCycle", string.Empty),
                            Weight = reader.GetFloat("ProductProperty_Weight"),
                            ServiceAssurance = reader.GetValueOrDefault("ProductProperty_ServiceAssurance", string.Empty),
                            LogisticsValuation = reader.GetInt32("ProductProperty_LogisticsValuation") != 0 ? (LogisticsValuationCode)reader.GetInt32("ProductProperty_LogisticsValuation") : LogisticsValuationCode.按重量,
                            Volume = reader.GetFloat("ProductProperty_Volume"),
                            Spec = reader.GetValueOrDefault("ProductProperty_Spec", string.Empty),
                            Desc = reader.GetValueOrDefault("ProductProperty_Desc", string.Empty),
                            DeliveryDesc = reader.GetValueOrDefault("ProductProperty_DeliveryDesc", string.Empty),
                            SupplierName = reader.GetValueOrDefault("ProductProperty_SupplierName", string.Empty),
                            ProductCategoryName = reader.GetValueOrDefault("CategoryName", string.Empty),
                        }
                    });
                }

                if (reader.NextResult() && reader.Read())
                {
                    total = Convert.ToInt32(reader["total"]);
                }
            }

            return list;
        }

        /// <summary>
        /// 构造Porduct数据查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private Tuple<string, MySqlParameter[]> BuildSqlGetProducts(ProductSearchDto search)
        {
            var parameters = new List<MySqlParameter>();
            string sql = "SELECT SQL_CALC_FOUND_ROWS *,productcategories.CategoryName CategoryName from products left join productcategories on products.ProductProperty_ProductCategoryId = productcategories.CategoryId  where products.IsDeleted=false ";
            if (search.ProductId.HasValue && search.ProductId.Value != Guid.Empty)
            {
                sql += " and products.ProductId = @ProductId";
                parameters.Add(new MySqlParameter("@ProductId", search.ProductId.Value));
            }
            if (!string.IsNullOrEmpty(search.ProductName))
            {
                sql += " and products.ProductProperty_ProductName like '%@ProductName%'";
                parameters.Add(new MySqlParameter("@ProductName", search.ProductName));
            }
            if (!string.IsNullOrEmpty(search.ProductNumber))
            {
                sql += " and products.ProductProperty_ProductNumber like '%@ProductNumber%'";
                parameters.Add(new MySqlParameter("@ProductNumber", search.ProductNumber));
            }
            if (!string.IsNullOrEmpty(search.SupplierName))
            {
                sql += " and products.ProductProperty_SupplierName like '%@SupplierName%'";
                parameters.Add(new MySqlParameter("@SupplierName", search.SupplierName));
            }
            if (!string.IsNullOrEmpty(search.SupplierNumber))
            {
                sql += " and products.ProductProperty_SupplierNumber like '%@SupplierNumber%'";
                parameters.Add(new MySqlParameter("@SupplierNumber", search.SupplierNumber));
            }
            if (search.Status != null)
            {
                string filter = "(";
                foreach (var item in search.Status)
                {
                    if (Enum.IsDefined(typeof(ProductStatusCode), item))
                    {
                        filter += string.Format(@",{0}", item.GetHashCode());
                    }
                }
                if (filter.Length > 1)
                {
                    filter = filter.Replace("(,", "");
                    sql += string.Format(@" and products.ProductProperty_ProductStatus in ({0})", filter);
                }
            }

            if (!string.IsNullOrEmpty(search.Filter))
            {
                sql += string.Format(@" and (ProductProperty_ProductName like '%{0}%' or 
                                ProductProperty_SupplierNumber like '%{0}%' or
                                ProductProperty_ProductModelType like '%{0}%')", search.Filter);
            }


            if (Enum.IsDefined(typeof(LoginRoleCode), search.UserRole))
            {
                if (search.UserRole == LoginRoleCode.供应商 && search.UserId.HasValue)
                {
                    sql += " and products.ProductProperty_SupplierId=@User";
                    parameters.Add(new MySqlParameter("@User", search.UserId.Value));
                }
            }

            sql += " order by products.CreatedTime desc";
            if (search.IsPage)
            {
                sql += " limit @startNum ,@PageSize";
                parameters.Add(new MySqlParameter("@startNum", (search.Page - 1) * search.Rows));
                parameters.Add(new MySqlParameter("@PageSize", search.Rows));
            }

            sql = string.Format("{0} ;SELECT FOUND_ROWS() AS total;", sql);

            return Tuple.Create(sql, parameters.ToArray());
        }

        /// <summary>
        /// 加载临时Goods信息
        /// </summary>
        public List<TempGoodsDto> LoadTempGoodsByProductId(Guid productId)
        {
            string sql = "select *  from tempgoods where ProductId='" + productId + "'";
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql);
            var list = new List<TempGoodsDto>();
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new TempGoodsDto
                    {
                        ProductId = reader.GetGuid("ProductId"),
                        TempGoodsId = reader.GetGuid("TempGoodsId"),
                        Sku = reader.GetValueOrDefault("Sku", string.Empty),
                        SystemSku = reader.GetValueOrDefault("SystemSku", string.Empty),
                        SkuStandard = reader.GetValueOrDefault("SkuStandard", string.Empty)
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 加载图片信息
        /// </summary>
        public List<GoodsPictureDto> LoadPicturesByTempGoodsId(Guid tempGoodsId)
        {
            string sql = "select *  from goodspicture where TempGoods_TempGoodsId='" + tempGoodsId + "'";
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql);
            var list = new List<GoodsPictureDto>();
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new GoodsPictureDto
                    {
                        GoodsPictureId = reader.GetGuid("GoodsPictureId"),
                        SmallFilePath = reader.GetValueOrDefault("SmallFilePath", string.Empty),
                        IsPrimary = reader.GetBoolean("IsPrimary"),
                        FilePath = reader.GetValueOrDefault("FilePath", string.Empty),
                        ServerUrl = reader.GetValueOrDefault("ServerUrl", string.Empty)
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 加载图片信息
        /// </summary>
        public List<GoodsPictureDto> LoadPicturesByGoodsId(Guid goodsId)
        {
            string sql = "select *  from goodspicture where Goods_GoodsId='" + goodsId + "'";
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql);
            var list = new List<GoodsPictureDto>();
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new GoodsPictureDto
                    {
                        GoodsPictureId = reader.GetGuid("GoodsPictureId"),
                        SmallFilePath = reader.GetValueOrDefault("SmallFilePath", string.Empty),
                        IsPrimary = reader.GetBoolean("IsPrimary"),
                        FilePath = reader.GetValueOrDefault("FilePath", string.Empty),
                        ServerUrl = reader.GetValueOrDefault("ServerUrl", string.Empty)
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 物理删除Pirces by TempGoods
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int DeletePircesByTempGoodsId(Guid tempGoodsId)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"Delete from goodsprices where TempGoods_TempGoodsId=@GoodsId ";
            parameters.Add(new MySqlParameter("@GoodsId", tempGoodsId));
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 加载TempGoods价格信息
        /// </summary>
        public List<GoodsPriceDto> LoadPircesByTempGoodsId(Guid tempGoodsId)
        {
            string sql = "select *  from goodsprices where TempGoods_TempGoodsId='" + tempGoodsId + "' order by StartNumber asc";
            return ReadPirces(sql);
        }

        /// <summary>
        /// 加载Product价格信息
        /// </summary>
        public List<GoodsPriceDto> LoadPircesByProductId(Guid productId)
        {
            string sql = "select *  from goodsprices where Product_ProductId='" + productId + "' order by StartNumber asc";
            return ReadPirces(sql);
        }

        /// <summary>
        /// 加载Product选择属性信息
        /// </summary>
        public List<ProductSelectedAttibutesDto> LoadSelectedAttibutesByProductId(Guid productId)
        {
            string sql = "select b.AttributeName,c.AttributeValue,a.ProductAttributeId,a.ProductAttributeValueId,a.ProductId,a.ProductToAttibutesId  from ProductSelectedAttibutes as a left join productattributes as b on a.ProductAttributeId = b.AttributeId left join attributevalues as c on c.AttributeValueId = a.ProductAttributeValueId where ProductId='" + productId + "'";
            return ReadSelectedAttibutes(sql);
        }

        /// <summary>
        /// 物理删除配送信息 by Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int DeleteDeliveryinfoByProductId(Guid productId)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"Delete from deliveryinfo where ProductId=@productId ;";
            parameters.Add(new MySqlParameter("@productId", productId));
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 物理删除Pirces by Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int DeletePircesByProductId(Guid productId)
        {
            var parameters = new List<MySqlParameter>();
            const string sql = @"Delete from goodsprices where Product_ProductId=@productId ;";
            parameters.Add(new MySqlParameter("@productId", productId));
            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 加载Goods价格信息
        /// </summary>
        public List<GoodsPriceDto> LoadPircesByGoodsId(Guid goodsId)
        {
            string sql = "select *  from goodsprices where Goods_GoodsId='" + goodsId + "';";
            return ReadPirces(sql);
        }

        /// <summary>
        /// 读取价格信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private List<GoodsPriceDto> ReadPirces(string sql)
        {
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql);
            var list = new List<GoodsPriceDto>();
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new GoodsPriceDto
                    {
                        GoodsPriceId = reader.GetGuid("GoodsPriceId"),
                        StartNumber = reader.GetInt32("StartNumber"),
                        EndNumber = reader.GetInt32("EndNumber"),
                        Price = reader.GetDecimal("Price")
                    });
                }
            }

            return list;
        }
        /// <summary>
        /// 读取产品选择属性信息
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private List<ProductSelectedAttibutesDto> ReadSelectedAttibutes(string sql)
        {
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql);
            var list = new List<ProductSelectedAttibutesDto>();
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(new ProductSelectedAttibutesDto
                    {
                        ProductAttibuteName = reader.GetValueOrDefault("AttributeName", string.Empty),
                        ProductAttributeValueName = reader.GetValueOrDefault("AttributeValue", string.Empty),
                        ProductToAttibutesId = reader.GetGuid("ProductToAttibutesId"),
                        ProductAttributeId = reader.GetGuid("ProductAttributeId"),
                        ProductAttributeValueId = reader.GetGuid("ProductAttributeValueId"),
                        ProductId = reader.GetGuid("ProductId")
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// 构造Goods
        /// </summary>
        /// <param name="goods"></param>
        /// <returns></returns>
        public int BiuldGoods(GoodsDto goods)
        {
            var parameters = new List<MySqlParameter>();
            string sql = @" INSERT INTO goods(GoodsId, Sku,SystemSku,SkuStandard, LinkStr,GoodsNumber, MinPrice,ProductId,CreatedTime,CreatedUser,ProductProperty_ProductName,
ProductProperty_ProductNumber,ProductProperty_ProductCategoryId,ProductProperty_SupplierId,ProductProperty_SupplierNumber,ProductProperty_ProductModelType,ProductProperty_ProductStatus,ProductProperty_SeoKeyWords,ProductProperty_SeoDesc,ProductProperty_MeansureUnit,ProductProperty_DeliveryCycle,ProductProperty_Weight,ProductProperty_ServiceAssurance,ProductProperty_LogisticsValuation,ProductProperty_Volume,ProductProperty_Spec,ProductProperty_Desc,ProductProperty_DeliveryDesc,ProductProperty_SupplierName) 
VALUES (@GoodsId,@Sku,@SystemSku,@SkuStandard,@LinkStr,@GoodsNumber,@MinPrice,@ProductId,@CreatedTime,@CreatedUser,@ProductProperty_ProductName,@ProductProperty_ProductNumber,@ProductProperty_ProductCategoryId,@ProductProperty_SupplierId,@ProductProperty_SupplierNumber,@ProductProperty_ProductModelType,@ProductProperty_ProductStatus,@ProductProperty_SeoKeyWords,@ProductProperty_SeoDesc,@ProductProperty_MeansureUnit,@ProductProperty_DeliveryCycle,@ProductProperty_Weight,@ProductProperty_ServiceAssurance,@ProductProperty_LogisticsValuation,@ProductProperty_Volume,@ProductProperty_Spec,@ProductProperty_Desc,@ProductProperty_DeliveryDesc,@ProductProperty_SupplierName) ;";

            parameters.Add(new MySqlParameter("@GoodsId", goods.GoodsId));
            parameters.Add(new MySqlParameter("@Sku", goods.Sku));
            parameters.Add(new MySqlParameter("@LinkStr", goods.LinkStr));
            parameters.Add(new MySqlParameter("@GoodsNumber", goods.GoodsNumber));
            parameters.Add(new MySqlParameter("@MinPrice", goods.MinPrice));
            parameters.Add(new MySqlParameter("@SystemSku", goods.SystemSku));
            parameters.Add(new MySqlParameter("@SkuStandard", goods.SkuStandard));
            parameters.Add(new MySqlParameter("@ProductId", goods.ProductId));
            parameters.Add(new MySqlParameter("@CreatedTime", goods.CreatedTime));
            parameters.Add(new MySqlParameter("@CreatedUser", goods.CreatedUser));
            parameters.Add(new MySqlParameter("@ProductProperty_ProductName", goods.ProductProperty.ProductName));
            parameters.Add(new MySqlParameter("@ProductProperty_ProductNumber", goods.ProductProperty.ProductNumber));
            parameters.Add(new MySqlParameter("@ProductProperty_ProductCategoryId", goods.ProductProperty.ProductCategoryId));
            parameters.Add(new MySqlParameter("@ProductProperty_SupplierId", goods.ProductProperty.SupplierId));
            parameters.Add(new MySqlParameter("@ProductProperty_SupplierNumber", goods.ProductProperty.SupplierNumber));
            parameters.Add(new MySqlParameter("@ProductProperty_ProductModelType", goods.ProductProperty.ProductModelType));
            parameters.Add(new MySqlParameter("@ProductProperty_ProductStatus", (int)goods.ProductProperty.ProductStatus));
            parameters.Add(new MySqlParameter("@ProductProperty_SeoKeyWords", goods.ProductProperty.SeoKeyWords));
            parameters.Add(new MySqlParameter("@ProductProperty_SeoDesc", goods.ProductProperty.SeoDesc));
            parameters.Add(new MySqlParameter("@ProductProperty_MeansureUnit", (int)goods.ProductProperty.MeansureUnit));
            parameters.Add(new MySqlParameter("@ProductProperty_DeliveryCycle", goods.ProductProperty.DeliveryCycle));
            parameters.Add(new MySqlParameter("@ProductProperty_Weight", goods.ProductProperty.Weight));
            parameters.Add(new MySqlParameter("@ProductProperty_ServiceAssurance", goods.ProductProperty.ServiceAssurance));
            parameters.Add(new MySqlParameter("@ProductProperty_LogisticsValuation", (int)goods.ProductProperty.LogisticsValuation));
            parameters.Add(new MySqlParameter("@ProductProperty_Volume", goods.ProductProperty.Volume));
            parameters.Add(new MySqlParameter("@ProductProperty_Spec", goods.ProductProperty.Spec));
            parameters.Add(new MySqlParameter("@ProductProperty_Desc", goods.ProductProperty.Desc));
            parameters.Add(new MySqlParameter("@ProductProperty_DeliveryDesc", goods.ProductProperty.DeliveryDesc));
            parameters.Add(new MySqlParameter("@ProductProperty_SupplierName", goods.ProductProperty.SupplierName));

            return MySqlHelper.ExecuteNonQuery(ConfigManager.DbConnectString, sql, parameters.ToArray());
        }

        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<GoodsDto> GetGoods(GoodsSearchDto search, out int total)
        {
            var sql = BuildSqlGetGoods(search);
            return ReadGoods(sql, out total);
        }

        /// <summary>
        /// Get GoodsNumbers
        /// </summary>
        /// <returns></returns>
        public List<int> GetGoodsNumbers()
        {
            string sql = "SELECT GoodsNumber from goods ";
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql, null);
            var list = new List<int>();
            using (reader)
            {
                while (reader.Read())
                {
                    list.Add(reader.GetInt32("GoodsNumber"));
                }
            }

            return list;
        }

        /// <summary>
        /// 数据查询及对象映射
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private List<GoodsDto> ReadGoods(Tuple<string, MySqlParameter[]> sql, out int total)
        {
            // 执行数据库查询
            var reader = MySqlHelper.ExecuteReader(ConfigManager.DbConnectString, sql.Item1, sql.Item2);
            var list = new List<GoodsDto>();
            total = 0;
            using (reader)
            {
                while (reader.Read())
                {
                    try
                    {
                        list.Add(new GoodsDto
                        {
                            GoodsId = reader.GetGuid("GoodsId"),
                            ProductId = reader.GetGuid("ProductId"),
                            MinPrice = reader.GetDecimal("MinPrice"),
                            SystemSku = reader.GetValueOrDefault("SystemSku", string.Empty),
                            SkuStandard = reader.GetValueOrDefault("SkuStandard", string.Empty),
                            GoodsNumber = reader.GetInt32("GoodsNumber"),
                            ScaningCount = reader.GetInt32("ScaningCount"),
                            AlreadySalesCount = reader.GetInt32("AlreadySalesCount"),
                            LinkStr = reader.GetValueOrDefault("LinkStr", string.Empty),
                            CreatedTime = reader.GetDateTime("CreatedTime"),
                            CreatedUser = reader.GetValueOrDefault<Guid?>("CreatedUser", null),
                            ProductProperty = new ProductPropertyDto
                            {
                                ProductName = reader.GetValueOrDefault("ProductProperty_ProductName", string.Empty),
                                ProductCategoryId = reader.GetGuid("ProductProperty_ProductCategoryId"),
                                SupplierId = reader.GetGuid("ProductProperty_SupplierId"),
                                SupplierNumber = reader.GetValueOrDefault("ProductProperty_SupplierNumber", string.Empty),
                                ProductModelType = reader.GetValueOrDefault("ProductProperty_ProductModelType", string.Empty),
                                ProductStatus = (ProductStatusCode)reader.GetInt32("ProductProperty_ProductStatus"),
                                SeoKeyWords = reader.GetValueOrDefault("ProductProperty_SeoKeyWords", string.Empty),
                                SeoDesc = reader.GetValueOrDefault("ProductProperty_SeoDesc", string.Empty),
                                MeansureUnit = reader.GetInt32("ProductProperty_MeansureUnit") != 0 ? (MeansureUnitCode)reader.GetInt32("ProductProperty_MeansureUnit") : MeansureUnitCode.个,
                                DeliveryCycle = reader.GetValueOrDefault("ProductProperty_DeliveryCycle", string.Empty),
                                Weight = reader.GetFloat("ProductProperty_Weight"),
                                ServiceAssurance = reader.GetValueOrDefault("ProductProperty_ServiceAssurance", string.Empty),
                                LogisticsValuation = reader.GetInt32("ProductProperty_LogisticsValuation") != 0 ? (LogisticsValuationCode)reader.GetInt32("ProductProperty_LogisticsValuation") : LogisticsValuationCode.按重量,
                                Volume = reader.GetFloat("ProductProperty_Volume"),
                                Spec = reader.GetValueOrDefault("ProductProperty_Spec", string.Empty),
                                Desc = reader.GetValueOrDefault("ProductProperty_Desc", string.Empty),
                                DeliveryDesc = reader.GetValueOrDefault("ProductProperty_DeliveryDesc", string.Empty),
                                SupplierName = reader.GetValueOrDefault("ProductProperty_SupplierName", string.Empty),
                            }
                        });
                    }
                    catch (Exception ex)
                    {

                    }
                }

                if (reader.NextResult() && reader.Read())
                {
                    total = Convert.ToInt32(reader["total"]);
                }
            }

            return list;
        }

        /// <summary>
        /// 构造Goods数据查询
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private Tuple<string, MySqlParameter[]> BuildSqlGetGoods(GoodsSearchDto search)
        {
            var parameters = new List<MySqlParameter>();
            string sql = "SELECT SQL_CALC_FOUND_ROWS GoodsId, Sku,SystemSku,SkuStandard, LinkStr,GoodsNumber, MinPrice,AlreadySalesCount,ScaningCount,t.ProductId,CreatedTime,CreatedUser,ProductProperty_ProductName,ProductProperty_ProductNumber,ProductProperty_ProductCategoryId,ProductProperty_SupplierId,ProductProperty_SupplierNumber,ProductProperty_ProductModelType,ProductProperty_ProductStatus,ProductProperty_SeoKeyWords,ProductProperty_SeoDesc,ProductProperty_MeansureUnit,ProductProperty_DeliveryCycle,ProductProperty_Weight,ProductProperty_ServiceAssurance,ProductProperty_LogisticsValuation,ProductProperty_Volume,ProductProperty_Spec,ProductProperty_Desc,ProductProperty_DeliveryDesc,ProductProperty_SupplierName from goods as t where 1=1 ";

            if (search.ProductCategoryId.HasValue && search.ProductCategoryId.Value != Guid.Empty)
            {
                sql += " and ProductProperty_ProductCategoryId = @ProductCategoryId";
                parameters.Add(new MySqlParameter("@ProductCategoryId", search.ProductCategoryId.Value));
            }

            if (search.ProductId.HasValue && search.ProductId.Value != Guid.Empty)
            {
                sql += " and t.ProductId = @ProductId";
                parameters.Add(new MySqlParameter("@ProductId", search.ProductId.Value));
            }

            if (search.GoodsId.HasValue && search.GoodsId.Value != Guid.Empty)
            {
                sql += " and t.GoodsId = @GoodsId";
                parameters.Add(new MySqlParameter("@GoodsId", search.GoodsId.Value));
            }

            if (search.GoodsNumber.HasValue && search.GoodsNumber.Value != 0)
            {
                sql += " and t.GoodsNumber = @GoodsNumber";
                parameters.Add(new MySqlParameter("@GoodsNumber", search.GoodsNumber.Value));
            }

            if (!string.IsNullOrEmpty(search.Filter))
            {
                sql += string.Format(@" and (ProductProperty_ProductName like '%{0}%' or 
                                t.SkuStandard like '%{0}%' or
                                t.ProductProperty_SupplierNumber like '%{0}%' or
                                t.ProductProperty_ProductModelType like '%{0}%')", search.Filter);
            }

            if (search.PirceMin.HasValue)
            {
                sql += " and t.PirceMin >= @PirceMin";
                parameters.Add(new MySqlParameter("@PirceMin", search.PirceMin.Value));
            }

            if (search.PirceMax.HasValue)
            {
                sql += " and t.PirceMin <= @PirceMax";
                parameters.Add(new MySqlParameter("@PirceMax", search.PirceMax.Value));
            }

            if (search.Status != null)
            {
                string filter = "(";
                foreach (var item in search.Status)
                {
                    if (Enum.IsDefined(typeof(ProductStatusCode), item))
                    {
                        filter += string.Format(",{0}", (int)item);
                    }
                }
                if (filter.Length > 1)
                {
                    filter = filter.Replace("(,", "");
                    sql += string.Format(@" and t.ProductProperty_ProductStatus in ({0})", filter);
                }
            }

            if (search.UserRole == LoginRoleCode.供应商 && search.UserId.HasValue)
            {
                sql += " and t.CreatedUser=@userId";
                parameters.Add(new MySqlParameter("@userId", search.UserId.Value));
            }

            #region 配送
            if (!string.IsNullOrEmpty(search.DelieveryArea))
            {
                //后期需要优化
                search.DelieveryArea += ",";
                search.DelieveryArea = search.DelieveryArea.Replace('-', ',').Replace(",,", string.Empty);
                sql += "  and EXISTS (select s.ProductId from deliveryinfo AS s where s.ProvinceCode IN (" + search.DelieveryArea + ") AND s.ProductId=t.ProductId )";
            }
            #endregion

            #region 属性
            if (!string.IsNullOrEmpty(search.AttribteFilter))
            {
                //后期需要优化
                foreach (var item in search.AttribteFilter.Split('-'))
                {
                    if (string.IsNullOrEmpty(item))
                    {
                        continue;
                    }

                    sql += " and EXISTS(select a.Goods_GoodsId from goodsattributevalues as a where a.Goods_GoodsId=t.GoodsId  and a.ProductAttributeNumber=" + item.Split('.')[0] + " and a.AttributeValueNumber =" + item.Split('.')[1] + ") ";
                }
            }
            #endregion 

            #region 排序
            if (search.OrderCode.HasValue)
            {
                switch (search.OrderCode.Value)
                {
                    case 0:
                        sql += " order by CreatedTime desc";
                        break;
                    case 1:
                        sql += " order by PirceMin desc";
                        break;
                    case -1:
                        sql += " order by PirceMin asc";
                        break;
                    case 2:
                        sql += " order by AlreadySalesCount desc";
                        break;
                    case -2:
                        sql += " order by AlreadySalesCount asc";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                sql += " order by CreatedTime desc";
            }
            #endregion

            #region 分页
            if (search.IsPage)
            {
                sql += " limit @startNum ,@PageSize";
                parameters.Add(new MySqlParameter("@startNum", (search.Page - 1) * search.Rows));
                parameters.Add(new MySqlParameter("@PageSize", search.Rows));
            }

            sql = string.Format("{0} ;SELECT FOUND_ROWS() AS total;", sql);
            #endregion

            return Tuple.Create(sql, parameters.ToArray());
        }

        #endregion
    }
}