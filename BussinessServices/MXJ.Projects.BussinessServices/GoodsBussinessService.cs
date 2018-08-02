using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MXJ.Core.Infrastructure.Caching;
using MXJ.Core.ValueInjector;
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Domain.Enums;
using MXJ.Projects.Domain.Models;
using MXJ.Projects.IBussinessServices;
using MXJ.Projects.IRepositoryServices;

namespace MXJ.Projects.BussinessServices
{
    /// <summary>
    /// 商品业务层
    /// </summary>
    public class GoodsBussinessService : BaseBussinessService, IGoodsBussinessService
    {
        protected IGoodsRepositoryService GoodsRepo { set; get; }

        public GoodsBussinessService(IGoodsRepositoryService goodsRepo)
        {
            GoodsRepo = goodsRepo;
        }

        #region 商品品类

        /// <summary>
        /// 获取所有种类
        /// </summary>
        /// <returns></returns>
        public ResponseBase<List<ProductCategoryDto>> GetProductCategories(ProductCategorySearchDto search)
        {
            var response = new ResponseBase<List<ProductCategoryDto>>();

            var result = CacheManager.Get(CacheKeyConstantVariable.ProductCategory, () =>
            {
                return GoodsRepo.GetProductCategories();
            }, TimeSpan.FromDays(10));

            if (search.CategoryNumber.HasValue)
            {
                result = result.Where(t => t.CategoryNumber == search.CategoryNumber.Value).ToList();
            }

            if (!string.IsNullOrEmpty(search.CategoryName))
            {
                result = result.Where(t => t.CategoryName.Contains(search.CategoryName)).ToList();
            }

            if (search.CategoryId.HasValue && search.CategoryId.Value != Guid.Empty)
            {
                result = result.Where(t => t.CategoryId == search.CategoryId.Value).ToList();
            }

            if (search.ParentCategoryId.HasValue && search.ParentCategoryId.Value != Guid.Empty)
            {
                result = result.Where(t => t.ParentCategoryId == search.ParentCategoryId.Value).ToList();
            }

            if (search.CategoryLevel.HasValue && search.CategoryLevel.Value != -1)
            {
                result = result.Where(t => t.CategoryLevel == search.CategoryLevel.Value).ToList();
            }

            response.Result = result;
            response.IsSuccess = true;

            return response;
        }

        /// <summary>
        /// 获取种类
        /// </summary>
        /// <returns></returns>
        public ResponseBase<ProductCategoryDto> GetProductCategoryById(Guid categoryId)
        {
            var response = new ResponseBase<ProductCategoryDto>();

            var result = CacheManager.Get(categoryId + "_" + CacheKeyConstantVariable.ProductCategory, () =>
            {
                var list = GetProductCategories(new ProductCategorySearchDto { CategoryId = categoryId }).Result;
                if (!list.Any())
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "系统没有找到任何相关结果";
                }

                if (list.Count > 1)
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "系统根据编号查询出结果不止一个，请核实查询条件";
                }

                var category = list.FirstOrDefault();
                category.ProductCategoryChildren = GetProductCategories(new ProductCategorySearchDto { ParentCategoryId = category.CategoryId }).Result;

                category.ProductAttributes = GetProductAttributeByCategory(category.CategoryId).Result;
                return category;
            }, TimeSpan.FromDays(10));

            response.Result = result;
            response.IsSuccess = true;
            return response;
        }

        /// <summary>
        /// 根据编号获取种类
        /// </summary>
        /// <returns></returns>
        public ResponseBase<ProductCategoryDto> GetProductCategoryByNumber(int number)
        {
            var response = new ResponseBase<ProductCategoryDto>();

            var result = CacheManager.Get(number + "_" + CacheKeyConstantVariable.ProductCategory, () =>
                  {
                      var list = GetProductCategories(new ProductCategorySearchDto { CategoryNumber = number }).Result;
                      if (!list.Any())
                      {
                          response.IsSuccess = false;
                          response.OperationDesc = "系统没有找到任何相关结果";
                      }

                      if (list.Count > 1)
                      {
                          response.IsSuccess = false;
                          response.OperationDesc = "系统根据编号查询出结果不止一个，请核实查询条件";
                      }

                      var category = list.First();
                      category.ProductCategoryChildren = GetProductCategories(new ProductCategorySearchDto { ParentCategoryId = category.CategoryId }).Result;

                      category.ProductAttributes = GetProductAttributeByCategory(category.CategoryId).Result;
                      return category;
                  }, TimeSpan.FromDays(10));

            response.Result = result;
            response.IsSuccess = true;
            return response;
        }

        /// <summary>
        /// 创建一个新的种类
        /// </summary>
        /// <param name="productCategoryDto"></param>
        /// <returns></returns>
        public ResponseBase<bool> CreateProductCategory(ProductCategoryDto productCategoryDto)
        {
            var response = new ResponseBase<bool>();
            using (var transactionScope = new TransactionScope())
            {
                if (IsExistProductCategory(productCategoryDto.CategoryName, productCategoryDto.ParentCategoryId))
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "名称已经被占用";
                    return response;
                }


                if (productCategoryDto.ParentCategoryId.HasValue && productCategoryDto.ParentCategoryId.Value != Guid.Empty)
                {
                    var parent = this.GetProductCategoryById(productCategoryDto.ParentCategoryId.Value);
                    if (parent == null || parent.Result == null)
                    {
                        response.IsSuccess = false;
                        response.OperationDesc = "数据异常，没有找到父级对象";
                        return response;
                    }
                    if (productCategoryDto.CategoryId == Guid.Empty)
                        productCategoryDto.CategoryId = Guid.NewGuid();
                    productCategoryDto.CategoryNumber = GoodsRepo.GetMaxCategoryNumberInProductCategory() + 1;
                    productCategoryDto.CategoryLevel = parent.Result.CategoryLevel + 1;
                    productCategoryDto.IsOurSale = parent.Result.IsOurSale;
                    productCategoryDto.ProductPath = parent.Result.ProductPath + productCategoryDto.CategoryId.ToString() + "_";
                    productCategoryDto.CreatedTime = DateTime.Now;
                    productCategoryDto.CreatedUser = CurrentAdminLogonUser != null ? CurrentAdminLogonUser.SysUserId : Guid.Empty;
                    //GoodsRepo.UpdateProductCategory(new ProductCategoryDto
                    //{
                    //    CategoryId = parent.Result.CategoryId,
                    //    CategoryName = parent.Result.CategoryName,
                    //    IsOurSale = parent.Result.IsOurSale,
                    //    ProductPath = parent.Result.ProductPath,
                    //    CategoryLevel = parent.Result.CategoryLevel,
                    //    UpdatedTime = DateTime.Now,
                    //    UpdatedUser = CurrentAdminLogonUser != null ? CurrentAdminLogonUser.SysUserId : Guid.Empty,
                    //});
                }
                else
                {
                    productCategoryDto.CategoryNumber = GoodsRepo.GetMaxCategoryNumberInProductCategory() + 1;
                    productCategoryDto.ProductPath = productCategoryDto.CategoryId.ToString() + "_";
                    productCategoryDto.CategoryLevel = 1;
                    productCategoryDto.CreatedTime = DateTime.Now;
                    productCategoryDto.CreatedUser = CurrentAdminLogonUser != null ? CurrentAdminLogonUser.SysUserId : Guid.Empty;
                }

                var result = GoodsRepo.CreateProductCategory(productCategoryDto);
                transactionScope.Complete();
                response.IsSuccess = result > 0;
                CacheManager.Remove(CacheKeyConstantVariable.ProductCategory);
                return response;
            }
        }

        /// <summary>
        ///     编辑商品种类
        /// </summary>
        /// <param name="productCategoryDto"></param>
        /// <returns></returns>
        public ResponseBase<bool> EditProductCategory(ProductCategoryDto productCategoryDto)
        {
            var response = new ResponseBase<bool>();
            using (var transactionScope = new TransactionScope())
            {
                if (IsExistProductCategory(productCategoryDto.CategoryName, productCategoryDto.ParentCategoryId))
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "名称已经被占用";
                    return response;
                }
                var model = this.GetProductCategoryById(productCategoryDto.CategoryId);
                if (model == null || model.Result == null)
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "数据异常，没有找到对象";
                    return response;
                }
                model.Result.CategoryName = productCategoryDto.CategoryName;
                model.Result.UpdatedTime = DateTime.Now;
                model.Result.UpdatedUser = CurrentAdminLogonUser != null ? CurrentAdminLogonUser.SysUserId : Guid.Empty;
                var result = GoodsRepo.UpdateProductCategory(model.Result);
                transactionScope.Complete();
                response.IsSuccess = result > 0;
                CacheManager.Remove(CacheKeyConstantVariable.ProductCategory);
                return response;
            }
        }

        /// <summary>
        /// 获取所有种类的树形结构树
        /// </summary>
        /// <returns></returns>
        public ResponseBase<List<ProductCategoryDto>> GetProductcategory()
        {
            var response = new ResponseBase<List<ProductCategoryDto>>();
            response = GoodsRepo.GetProductcategory();
            response.IsSuccess = true;
            return response;
        }

        /// <summary>
        /// 验证名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private bool IsExistProductCategory(string name, Guid? parentId)
        {
            return GetProductCategories(new ProductCategorySearchDto
            {
                CategoryName = name,
                ParentCategoryId = parentId
            }).Result.Any();
        }
        #endregion

        #region 商品属性

        /// <summary>
        ///     创建一个新的属性
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResponseBase<bool> CreateProductAttribute(ProductAttributeDto dto)
        {
            var response = new ResponseBase<bool>();
            using (var transactionScope = new TransactionScope())
            {
                if (IsExistProductAttribute(dto.AttributeName, dto.ProductCategoryId))
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "属性已经被占用";
                    return response;
                }

                var category = this.GetProductCategoryById(dto.ProductCategoryId);
                if (category == null || category.Result == null)
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "数据异常，没有找到对应类目";
                    return response;
                }
                if (dto.AttributeId == Guid.Empty)
                    dto.AttributeId = Guid.NewGuid();
                dto.AttributeNumber = GoodsRepo.GetMaxAttributeNumberInProductAttribute() + 1;
                dto.CreatedTime = DateTime.Now;
                dto.CreatedUser = CurrentAdminLogonUser != null ? CurrentAdminLogonUser.SysUserId : Guid.Empty;

                var result = GoodsRepo.CreateProductAttribute(dto);
                transactionScope.Complete();
                response.IsSuccess = result > 0;
                CacheManager.Remove(CacheKeyConstantVariable.ProductCategory);
                return response;
            }
        }

        /// <summary>
        /// 获取属性值信息
        /// </summary>
        /// <param name="id">属性值ID</param>
        /// <returns></returns>
        public ResponseBase<AttributeValuesDto> GetAttributeValueByID(Guid id)
        {
            var response = new ResponseBase<AttributeValuesDto>();

            var model = GoodsRepo.GetAttributeValueByID(id);
            if (model != null)
            {
                response.IsSuccess = true;
                response.Result = model;
            }
            else
            {
                response.IsSuccess = false;
            }
            return response;
        }

        /// <summary>
        ///     创建一个新的属性值
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResponseBase<bool> CreateProductAttributeValue(AttributeValuesDto dto)
        {
            var response = new ResponseBase<bool>();
            using (var transactionScope = new TransactionScope())
            {
                if (IsExistAttributeValue(dto))
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "属性值已经被占用";
                    return response;
                }


                if (dto.AttributeValueId == Guid.Empty)
                    dto.AttributeValueId = Guid.NewGuid();
                dto.AttributeValueNumber = GoodsRepo.GetMaxAttributeValueNumberInAttributeValue() + 1;
                dto.CreatedTime = DateTime.Now;
                dto.CreatedUser = CurrentAdminLogonUser != null ? CurrentAdminLogonUser.SysUserId : Guid.Empty;

                var result = GoodsRepo.CreateAttributeValue(dto);
                transactionScope.Complete();
                response.IsSuccess = result > 0;
                return response;
            }
        }

        /// <summary>
        ///     编辑属性值
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResponseBase<bool> EditProductAttributeValue(AttributeValuesDto dto)
        {
            var response = new ResponseBase<bool>();
            using (var transactionScope = new TransactionScope())
            {

                if (IsExistAttributeValue(dto))
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "属性值已经被占用";
                    return response;
                }

                dto.UpdatedTime = DateTime.Now;
                dto.UpdatedUser = CurrentAdminLogonUser != null ? CurrentAdminLogonUser.SysUserId : Guid.Empty;

                var result = GoodsRepo.EditProductAttributeValue(dto);
                transactionScope.Complete();
                response.IsSuccess = result > 0;
                return response;
            }
        }

        /// <summary>
        /// 校验属性值是否存在
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private bool IsExistAttributeValue(AttributeValuesDto dto)
        {
            var response = GoodsRepo.IsExistAttributeValue(dto);
            return response.Result;
        }

        /// <summary>
        ///     编辑属性
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResponseBase<bool> EditProductAttribute(ProductAttributeDto dto)
        {
            var response = new ResponseBase<bool>();
            using (var transactionScope = new TransactionScope())
            {
                if (IsExistProductAttribute(dto.AttributeName, dto.ProductCategoryId))
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "属性已经被占用";
                    return response;
                }
                var model = GetProductAttributeByID(dto.AttributeId);
                if (model == null)
                {
                    response.IsSuccess = false;
                    response.OperationDesc = "属性未找到";
                    return response;
                }
                dto.RowVersion = model.Result.RowVersion;
                dto.IsDeleted = model.Result.IsDeleted;
                dto.CreatedTime = model.Result.CreatedTime;
                dto.CreatedUser = model.Result.CreatedUser;
                dto.CreatedUserPath = model.Result.CreatedUserPath;
                dto.UpdatedTime = DateTime.Now;
                dto.UpdatedUser = CurrentAdminLogonUser != null ? CurrentAdminLogonUser.SysUserId : Guid.Empty;

                var result = GoodsRepo.EditProductAttribute(dto);
                transactionScope.Complete();
                response.IsSuccess = result > 0;
                // CacheManager.Remove(CacheKeyConstantVariable.ProductCategory);
                return response;
            }
        }

        /// <summary>
        ///     逻辑删除属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseBase<bool> DeleteProductAttribute(Guid id)
        {
            var response = new ResponseBase<bool>();
            using (var transactionScope = new TransactionScope())
            {
                var result = GoodsRepo.DeleteProductAttribute(id);
                transactionScope.Complete();
                response.IsSuccess = result > -1;
                return response;
            }
        }

        /// <summary>
        ///     逻辑删除属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseBase<bool> DeleteProductAttributeValue(Guid id)
        {
            var response = new ResponseBase<bool>();
            using (var transactionScope = new TransactionScope())
            {
                var result = GoodsRepo.DeleteProductAttributeValue(id);
                transactionScope.Complete();
                response.IsSuccess = result > 0;
                return response;
            }
        }

        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <returns></returns>
        public ResponseBase<ProductAttributeDto> GetProductAttributeByID(Guid attributeId)
        {
            var response = new ResponseBase<ProductAttributeDto>();
            response.Result = GoodsRepo.GetProductAttributeByID(attributeId);
            response.IsSuccess = true;
            return response;
        }

        /// <summary>
        /// 获取属性相关信息
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns></returns>
        public ResponseBase<List<ProductAttributeDto>> GetProductAttributeByCategory(Guid categoryId)
        {
            var response = new ResponseBase<List<ProductAttributeDto>>();
            var result = CacheManager.Get(CacheKeyConstantVariable.ProductCategory + "_" + categoryId, () =>
              {
                  return GoodsRepo.GetProductAttributeByCategory(categoryId);
              }, TimeSpan.FromDays(10));

            response.Result = result;
            response.IsSuccess = true;
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
            response = GoodsRepo.SearchProductAttribute(searchDto);
            response.IsSuccess = true;
            return response;
        }

        /// <summary>
        /// 验证名称是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private bool IsExistProductAttribute(string name, Guid categoryId)
        {
            return GoodsRepo.IsExistProductAttribute(name, categoryId);
        }

        #endregion

        #region 产品

        /// <summary>
        /// 查询Goods
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ResponseBase<List<GoodsDto>> GetGoods(GoodsSearchDto search, out int total)
        {
            var response = new ResponseBase<List<GoodsDto>>();
            total = 0;
            response.Result = GoodsRepo.GetGoods(search, out total);
            foreach (var goods in response.Result)
            {
                goods.Prices = GoodsRepo.LoadPircesByGoodsId(goods.GoodsId);
                goods.Pictures = GoodsRepo.LoadPicturesByGoodsId(goods.GoodsId);
                int temp = 0;
                if (search.IsLoadBotherGoods)
                {
                    goods.Brother = GoodsRepo.GetGoods(new GoodsSearchDto { ProductId = goods.ProductId, IsPage = false }, out temp).Where(t => t.ProductProperty.ProductStatus == ProductStatusCode.已上架).ToList();
                }

                if (search.IsLoadCousinBrotherGoods)
                {
                    goods.CousinBrother = GoodsRepo.GetGoods(new GoodsSearchDto { ProductCategoryId = goods.ProductProperty.ProductCategoryId, IsPage = true, Rows = 12 }, out temp).Where(t => t.ProductProperty.ProductStatus == ProductStatusCode.已上架).ToList();
                    goods.CousinBrother.ForEach(t => t.Pictures = GoodsRepo.LoadPicturesByGoodsId(t.GoodsId)); 
                }
            }

            response.IsSuccess = true;
            return response;
        }

        /// <summary>
        /// 获取单个Goods
        /// </summary>
        /// <param name="goodsNumber"></param>
        /// <returns></returns>
        public ResponseBase<GoodsDto> GetGoods(int goodsNumber)
        {
            var response = new ResponseBase<GoodsDto>();
            int temp = 0;

            var result = CacheManager.Get(goodsNumber.ToString() + CacheKeyConstantVariable.Goods, () =>
             {
                 return GetGoods(new GoodsSearchDto { GoodsNumber = goodsNumber, IsLoadCousinBrotherGoods = true, IsLoadBotherGoods = true, IsPage = false }, out temp).Result.First();
             }, TimeSpan.FromDays(1));

            response.Result = result;
            response.IsSuccess = true;
            return response;
        }

        private int GenerateGoodsNumber(List<int> existData)
        {
            var ra = new Random();
            var s = ra.Next(100000, int.MaxValue);
            if (existData.Exists(t => t == s))
            {
                return GenerateGoodsNumber(existData);
            }
            else
            {
                existData.Add(s);
                return s;
            }
        }

        /// <summary>
        /// 商品上架
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ResponseBase<ProductDto> ProductToGoods(Guid productId)
        {
            var response = new ResponseBase<ProductDto>();
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    var product = GetProductById(productId, true, true, true, true, true).Result;
                    if (product == null || !product.TempGoods.Any())
                    {
                        response.IsSuccess = false;
                        response.OperationDesc = "数据错误";
                        return response;
                    }

                    var GoodsNumbers = GoodsRepo.GetGoodsNumbers();
                    foreach (var tempGoods in product.TempGoods)
                    {
                        var goods = new GoodsDto();
                        goods.Inject(tempGoods);
                        goods.GoodsId = Guid.NewGuid();
                        goods.GoodsNumber = GenerateGoodsNumber(GoodsNumbers);
                        goods.ProductProperty = product.ProductProperty;
                        goods.ProductProperty.ProductStatus = ProductStatusCode.已上架;

                        GoodsRepo.BiuldGoods(goods);

                        #region  价格信息处理
                        goods.Prices = tempGoods.Prices;
                        goods.Prices = goods.Prices ?? new List<GoodsPriceDto>();
                        if (!goods.Prices.Any() && product.Prices != null)
                        {
                            goods.Prices = product.Prices;
                        }
                        if (!goods.Prices.Any())
                        {
                            response.IsSuccess = false;
                            response.OperationDesc = "商品没有检测到价格信息";
                            return response;
                        }

                        GoodsRepo.CreateGoodsPrices(goods);
                        goods.MinPrice = goods.Prices.Min(t => t.Price);
                        #endregion

                        #region  图片信息处理
                        goods.Pictures = tempGoods.Pictures;
                        if (goods.Pictures != null && goods.Pictures.Any())
                        {
                            GoodsRepo.CreatePictures(goods);
                        }
                        #endregion

                        #region 属性信息处理
                        var productCategory = this.GetProductCategoryById(goods.ProductProperty.ProductCategoryId).Result;
                        goods.GoodsAttributeValues = new List<GoodsAttributeValueDto>();
                        foreach (var attr in product.ProductSelectedAttibutes)
                        {
                            var attrData = productCategory.ProductAttributes.FirstOrDefault(t => t.AttributeId == attr.ProductAttributeId);
                            if (attrData == null)
                            {
                                continue;
                            }

                            var valueData = attrData.AttributeValues.FirstOrDefault(t => t.AttributeValueId == attr.ProductAttributeValueId);
                            if (valueData == null)
                            {
                                continue;
                            }

                            goods.GoodsAttributeValues.Add(new GoodsAttributeValueDto
                            {
                                AttributeId = attrData.AttributeId,
                                AttributeValueId = valueData.AttributeValueId,
                                AttributeNumber = attrData.AttributeNumber,
                                AttributeValueNumber = valueData.AttributeValueNumber
                            });
                        }

                        foreach (var attr in tempGoods.TempGoodsSelectedSalesAttibutes)
                        {
                            var attrData = productCategory.ProductAttributes.FirstOrDefault(t => t.AttributeId == attr.ProductAttributeId);
                            if (attrData == null)
                            {
                                continue;
                            }

                            var valueData = attrData.AttributeValues.FirstOrDefault(t => t.AttributeValueId == attr.ProductAttributeValueId);
                            if (valueData == null)
                            {
                                continue;
                            }

                            goods.GoodsAttributeValues.Add(new GoodsAttributeValueDto
                            {
                                AttributeId = attrData.AttributeId,
                                AttributeValueId = valueData.AttributeValueId,
                                AttributeNumber = attrData.AttributeNumber,
                                AttributeValueNumber = valueData.AttributeValueNumber
                            });
                        }

                        GoodsRepo.CreateGoodsAttributeValues(goods);
                        #endregion
                    }

                    //更新product状态
                    GoodsRepo.UpdateProductStatus(productId, ProductStatusCode.已上架);

                    transactionScope.Complete();
                    response.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    WriteLogException(ex);
                    response.IsSuccess = false;
                }

                response.OperationDesc = response.IsSuccess ? "上架成功" : "上架失败";
                return response;
            }
        }

        /// <summary>
        /// 商品未通过
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ResponseBase<ProductDto> ProductNoPass(Guid productId, string reason)
        {
            var response = new ResponseBase<ProductDto>();
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    //更新product状态
                    GoodsRepo.UpdateProductStatus(productId, ProductStatusCode.未通过);
                    GoodsRepo.UpdateProductNoPassReason(productId, reason);
                    transactionScope.Complete();
                    response.IsSuccess = true;
                }
                catch (Exception ex)
                {
                    WriteLogException(ex);
                    response.IsSuccess = false;
                }

                response.OperationDesc = response.IsSuccess ? "上架成功" : "上架失败";
                return response;
            }
        }

        /// <summary>
        /// 创建一个新的商品
        /// </summary>
        /// <param name="productCategoryDto"></param>
        /// <returns></returns>
        public ResponseBase<ProductDto> CreateProduct(ProductDto productDto)
        {
            var response = new ResponseBase<ProductDto>();
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    //判断是否是编辑
                    if (!productDto.IsEdit)
                    {
                        productDto.ProductProperty.ProductStatus = Domain.Enums.ProductStatusCode.草稿;
                        productDto.CreatedTime = DateTime.Now;
                        //productDto.CreatedUser = CurrentMemberUser != null ? CurrentMemberUser.MemberId : Guid.Empty;
                        productDto.IsDeleted = false;
                        var result = GoodsRepo.CreateProdut(productDto);
                        if (result <= 0)
                        {
                            throw new Exception("创建商品失败");
                        }
                        if (productDto.GeneralAttributes != null)
                        {
                            foreach (var item in productDto.GeneralAttributes)
                            {
                                ProductSelectedAttibutesDto selectedAttributeDto = new ProductSelectedAttibutesDto();
                                selectedAttributeDto.ProductToAttibutesId = Guid.NewGuid();
                                selectedAttributeDto.ProductAttributeId = item.AttributeId;
                                selectedAttributeDto.ProductAttributeValueId = item.SelectedAttributeValue;
                                selectedAttributeDto.ProductId = productDto.ProductId;
                                result = GoodsRepo.CreateProductSelectedAttibutes(selectedAttributeDto);
                                if (result <= 0)
                                {
                                    throw new Exception("创建商品属性失败");
                                }
                            }
                        }
                        transactionScope.Complete();
                        response.IsSuccess = result > 0;
                        response.Result = productDto;
                    }
                    else
                    {
                        var result = GoodsRepo.UpdateProductStepTwo(productDto);
                        if (result <= 0)
                        {
                            throw new Exception("修改商品失败");
                        }
                        //删除原来选定属性
                        result = GoodsRepo.DeleteProductSelectedAttributevVlues(productDto.ProductId);
                        if (productDto.GeneralAttributes != null)
                        {
                            foreach (var item in productDto.GeneralAttributes)
                            {
                                ProductSelectedAttibutesDto selectedAttributeDto = new ProductSelectedAttibutesDto();
                                selectedAttributeDto.ProductToAttibutesId = Guid.NewGuid();
                                selectedAttributeDto.ProductAttributeId = item.AttributeId;
                                selectedAttributeDto.ProductAttributeValueId = item.SelectedAttributeValue;
                                selectedAttributeDto.ProductId = productDto.ProductId;
                                result = GoodsRepo.CreateProductSelectedAttibutes(selectedAttributeDto);
                                if (result <= 0)
                                {
                                    throw new Exception("创建商品属性失败");
                                }
                            }
                        }

                        transactionScope.Complete();
                        response.IsSuccess = result > 0;
                        response.Result = productDto;
                    }
                }
                catch (Exception ex)
                {
                    WriteLogException(ex);
                    response.IsSuccess = false;
                }
                response.OperationDesc = response.IsSuccess ? "保存成功" : "保存失败";
                return response;
            }
        }

        /// <summary>
        /// 存储TempGoods
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public ResponseBase<ProductDto> CreateTempGoods(ProductDto productDto)
        {
            var response = new ResponseBase<ProductDto>();
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    if (!productDto.TempGoods.Any())
                    {
                        response.IsSuccess = false;
                        response.OperationDesc = "数据错误";
                        return response;
                    }

                    //验证销售属性的唯一性
                    var keyValues = new List<List<AttibutesAndValues>>();
                    foreach (var item in productDto.TempGoods)
                    {
                        var attrs = new List<AttibutesAndValues>();
                        foreach (var attr in item.SelectedAttibutesAndValues.Split('|'))
                        {
                            if (string.IsNullOrEmpty(attr) || attr.IndexOf(":") < 0)
                            {
                                continue;
                            }

                            var value = attr.Split(':');
                            var AttributeId = Guid.Parse(value[0]);
                            var AttributeValueId = Guid.Parse(value[1]);

                            attrs.Add(new AttibutesAndValues
                            {
                                AttributeId = AttributeId,
                                AttributeValueId = AttributeValueId
                            });
                        }

                        if (keyValues.Any())
                        {
                            foreach (var existItem in keyValues)
                            {
                                var isExist = new List<bool>();
                                foreach (var existAttr in existItem)
                                {
                                    isExist.Add(attrs.Any(t => t.AttributeId == existAttr.AttributeId && t.AttributeValueId == existAttr.AttributeValueId));
                                }

                                if (isExist.Count() == isExist.Where(t => t).Count())
                                {
                                    response.IsSuccess = false;
                                    response.OperationDesc = "不能存在相同属性的Sku";
                                    return response;
                                }
                            }

                            keyValues.Add(attrs);
                        }
                        else
                        {
                            keyValues.Add(attrs);
                        }
                    }

                    var product = GetProductById(productDto.ProductId, true, true, true, false).Result;
                    product.TempGoods.ForEach(oldTempGoods =>
                    {
                        //删除以前的价格
                        GoodsRepo.DeletePircesByTempGoodsId(oldTempGoods.TempGoodsId);
                        //删除以前的图片
                        GoodsRepo.DeletePicturesByTempGoodsId(oldTempGoods.TempGoodsId);
                        //删除以前的属性
                        GoodsRepo.DeleteAttributevVlues(new SelectedAttibutesDto { TempGoodsId = oldTempGoods.TempGoodsId });
                    });

                    //删除以前的TempGoods
                    GoodsRepo.DeleteTempGoodsByProductId(product.ProductId);
                    //删除以前的product价格
                    GoodsRepo.DeletePircesByProductId(product.ProductId);

                    foreach (var item in productDto.TempGoods)
                    {
                        item.TempGoodsId = Guid.NewGuid();
                        item.SystemSku = product.ProductProperty.SupplierNumber + item.Sku;
                    }

                    bool result = false;
                    foreach (var tempGoods in productDto.TempGoods)
                    {
                        result = GoodsRepo.CreateTempGoods(tempGoods) > 0;
                        if (!result)
                        {
                            throw new Exception("存储TempGoods发生异常");
                        }

                        if (!string.IsNullOrEmpty(tempGoods.SelectedAttibutesAndValues))
                        {
                            result = GoodsRepo.CreateTempGoodsAttributeValues(tempGoods) > 0;
                            if (!result)
                            {
                                throw new Exception("存储临时goods的销售属性及值发生异常");
                            }
                        }

                        if (tempGoods.Pictures != null && tempGoods.Pictures.Any())
                        {
                            result = GoodsRepo.CreatePictures(tempGoods) > 0;
                            if (!result)
                            {
                                throw new Exception("创建图片发生异常");
                            }
                        }
                    }

                    transactionScope.Complete();
                    response.IsSuccess = result;
                    response.Result = productDto;
                }
                catch (Exception ex)
                {
                    WriteLogException(ex);
                    response.IsSuccess = false;
                }

                response.OperationDesc = response.IsSuccess ? "保存成功" : "保存失败";
                return response;
            }
        }

        /// <summary>
        /// 获取单个商品
        /// </summary>
        /// <param name="id">ProductId</param>
        /// <param name="isLoadTempGoods">是否加TempGoods信息</param>
        /// <param name="isLoadPictures">是否加载图片信息</param>
        /// <param name="isLoadPirces">是否加载价格信息</param>
        /// <param name="isLoadDeliveryInfo">是否加载配送信息</param>
        /// <returns></returns>
        public ResponseBase<ProductDto> GetProductById(Guid id, bool isLoadTempGoods, bool isLoadPictures, bool isLoadPirces, bool isLoadDeliveryInfo, bool IsLoadSelectedAttibutes = false)

        {
            var response = new ResponseBase<ProductDto>();

            var result = GetProducts(new ProductSearchDto { ProductId = id, IsPage = false, IsLoadTempGoods = isLoadTempGoods, IsLoadPictures = isLoadPictures, IsLoadPirces = isLoadPirces, IsLoadDelivery = isLoadDeliveryInfo, IsLoadSelectedAttibutes = IsLoadSelectedAttibutes }).Result;

            if (!result.Any())
            {
                response.OperationDesc = "数据有误，没找到记录";
                response.IsSuccess = false;
                return response;
            }

            //TODO:获取其附属信息

            response.Result = result.First();
            response.IsSuccess = true;
            return response;
        }

        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public SearchResponseBase<List<ProductDto>> GetProducts(ProductSearchDto search)
        {
            var response = new SearchResponseBase<List<ProductDto>>();
            try
            {
                int total = 0;
                var result = GoodsRepo.GetProducts(search, out total);
                if (search.IsLoadSelectedAttibutes)
                {
                    result.ForEach(item =>
                    {
                        item.ProductSelectedAttibutes = GoodsRepo.LoadSelectedAttibutesByProductId(item.ProductId);
                    });
                }
                if (search.IsLoadTempGoods)
                {
                    result.ForEach(item =>
                    {
                        item.TempGoods = GoodsRepo.LoadTempGoodsByProductId(item.ProductId);
                        item.TempGoods.ForEach(t =>
                        {
                            var attr = GoodsRepo.GetAttributeValues(new SelectedAttibutesDto { TempGoodsId = t.TempGoodsId });
                            t.TempGoodsSelectedSalesAttibutes = attr;
                            t.SelectedAttibutesAndValues = string.Join("|", attr.Select(m => m.ProductAttributeId + ":" + m.ProductAttributeValueId));
                        });
                    });
                }

                if (search.IsLoadPirces)
                {
                    result.ForEach(item => item.Prices = GoodsRepo.LoadPircesByProductId(item.ProductId));
                    result.ForEach(item =>
                    {
                        item.TempGoods.ForEach(tempGoods => tempGoods.Prices = GoodsRepo.LoadPircesByTempGoodsId(tempGoods.TempGoodsId));
                    });
                }

                if (search.IsLoadPictures)
                {
                    result.ForEach(item =>
                    {
                        item.TempGoods.ForEach(tempGoods => tempGoods.Pictures = GoodsRepo.LoadPicturesByTempGoodsId(tempGoods.TempGoodsId));
                    });
                }

                if (search.IsLoadDelivery)
                {
                    result.ForEach(item => item.DeliveryInfo = GoodsRepo.GetDeliveryInfo(item.ProductId));
                }

                response.Result = result;
                response.Rows = search.Rows;
                response.Page = search.Page;
                response.TotalRecordCount = total;
                response.IsSuccess = true;

            }
            catch (Exception ex)
            {
                WriteLogException(ex);
                response.IsSuccess = false;
            }

            response.OperationDesc = "查询商品列表" + (response.IsSuccess ? "成功" : "失败");
            return response;
        }

        /// <summary>
        /// 获取商品个数
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ResponseBase<int> GetProductNumberByFilter(ProductSearchDto search)
        {
            var response = new ResponseBase<int>();
            var result = GoodsRepo.GetProductNumberByFilter(search);
            response.IsSuccess = result > -1;
            response.Result = result;
            return response;
        }

        /// <summary>
        /// 获取商品个数
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public ResponseBase<int> GetGoodsNumberByFilter(GoodsSearchDto search)
        {
            var response = new ResponseBase<int>();
            var result = GoodsRepo.GetGoodsNumberByFilter(search);
            response.IsSuccess = result > -1;
            response.Result = result;
            return response;
        }

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="ids">商品Id集合</param>
        /// <returns></returns>
        public ResponseBase<bool> DeleteProductById(List<Guid> ids)
        {
            var response = new ResponseBase<bool>();
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    var result = GoodsRepo.DeleteProductById(ids);
                    response.IsSuccess = result > -1;
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    WriteLogException(ex);
                    response.IsSuccess = false;
                }
                return response;
            }
        }

        /// <summary>
        /// 更新产品状态字段
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public ResponseBase<bool> UpdateProductStatus(Guid productId, ProductStatusCode ProductStatus)
        {
            var response = new ResponseBase<bool>();
            var result = GoodsRepo.UpdateProductStatus(productId, ProductStatus);
            response.IsSuccess = result > -1;
            return response;
        }

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="ids">商品Id集合</param>
        /// <param name="initStatus">初始状态</param>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="submintUser">更新用户</param>
        /// <returns></returns>
        public ResponseBase<bool> SetProductById(List<Guid> ids,
            ProductStatusCode? initStatus, ProductStatusCode targetStatus, Guid submintUser)
        {
            var response = new ResponseBase<bool>();

            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    var result = GoodsRepo.SetProductById(ids, initStatus, targetStatus, submintUser);
                    response.IsSuccess = result > -1;
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    WriteLogException(ex);
                    response.IsSuccess = false;
                }
                return response;
            }
        }

        /// <summary>
        /// 更新Goods商品状态
        /// </summary>
        /// <param name="ids">Goods商品Id集合</param>
        /// <param name="initStatus">初始状态</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns></returns>
        public ResponseBase<bool> SetGoodsStatusById(List<Guid> ids, ProductStatusCode? initStatus, ProductStatusCode targetStatus)
        {
            var response = new ResponseBase<bool>();
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    var result = GoodsRepo.SetGoodsStatusById(ids, initStatus, targetStatus);
                    response.IsSuccess = result > -1;
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    WriteLogException(ex);
                    response.IsSuccess = false;
                }
                return response;
            }
        }

        /// <summary>
        /// 第四步操作更新产品
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        public ResponseBase<Guid> UpdateProductSetpFour(ProductDto productDto)
        {
            ResponseBase<Guid> response = new ResponseBase<Guid>();
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    //更新基本信息
                    GoodsRepo.UpdateProductTradeInfo(productDto);
                    //删除历史价格信息
                    GoodsRepo.DeletePircesByProductId(productDto.ProductId);
                    if (productDto.TempGoods != null && productDto.TempGoods.Count > 0)
                    {
                        foreach (var item in productDto.TempGoods)
                        {
                            GoodsRepo.DeletePircesByTempGoodsId(item.TempGoodsId);
                            //新建价格信息
                            if (item.Prices != null && item.Prices.Count > 0)
                            {
                                GoodsRepo.CreateTempGoodsPrices(item);
                            }
                        }
                    }

                    productDto.Prices = productDto.Prices ?? new List<GoodsPriceDto>();
                    GoodsRepo.CreatePricesProduct(productDto);
                    transactionScope.Complete();
                    response.IsSuccess = true;
                    response.Result = productDto.ProductId;
                }
                catch (Exception ex)
                {
                    WriteLogException(ex);
                    response.IsSuccess = false;
                }
                response.OperationDesc = response.IsSuccess ? "保存成功" : "保存失败";
                return response;
            }
        }

        /// <summary>
        /// 获取Product配送信息
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns></returns>
        public ResponseBase<ProductDto> GetDeliveryInfo(Guid productId)
        {
            var response = new ResponseBase<ProductDto>();
            response.Result = this.GetProductById(productId, false, false, false, true).Result;
            return response;
        }

        /// <summary>
        /// 处理配送信息
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns></returns>
        public ResponseBase<ProductDto> ProcessProductDelivery(ProductDto productDto)
        {
            var response = new ResponseBase<ProductDto>();
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    productDto.ProductProperty = productDto.ProductProperty ?? new ProductPropertyDto();
                    if (GoodsRepo.UpdateProductDeliveryDesc(productDto) > 0)
                    {
                        GoodsRepo.DeleteDeliveryinfoByProductId(productDto.ProductId);
                        response.IsSuccess = GoodsRepo.ProcessProductDelivery(productDto) > 0;
                    }

                    transactionScope.Complete();
                    response.Result = productDto;
                }
                catch (Exception ex)
                {
                    WriteLogException(ex);
                    response.IsSuccess = false;
                }
            }

            response.OperationDesc = "操作物理配送信息" + (response.IsSuccess ? "成功" : "失败");
            return response;
        }
        /// <summary>
        /// 更新产品详细
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns></returns>
        public ResponseBase<ProductDto> UpdateProductStepFive(ProductDto productDto)
        {
            var response = new ResponseBase<ProductDto>();
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    productDto.ProductProperty = productDto.ProductProperty ?? new ProductPropertyDto();
                    var result = GoodsRepo.UpdateProductStepFive(productDto);
                    if (result <= 0)
                    {
                        throw new Exception("更新产品详细信息出错！");
                    }

                    transactionScope.Complete();
                    response.IsSuccess = true;
                    response.Result = productDto;
                }
                catch (Exception ex)
                {
                    WriteLogException(ex);
                    response.IsSuccess = false;
                }
            }
            response.OperationDesc = "更新产品详细信息" + (response.IsSuccess ? "成功" : "失败");
            return response;
        }
        #endregion

    }

    public class AttibutesAndValues
    {
        public Guid AttributeId { set; get; }
        public Guid AttributeValueId { set; get; }
    }
}
