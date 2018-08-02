using System;
using System.Collections.Generic;
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Domain.Enums;
using MXJ.Projects.Domain.Models;

namespace MXJ.Projects.IRepositoryServices
{
    /// <summary>
    /// 商品持久化层
    /// </summary>
    public interface IGoodsRepositoryService : IBaseRepositoryService
    {
        #region 商品品类

        /// <summary>
        /// 查询商品分类
        /// </summary> 
        /// <param name="search"></param> 
        /// <returns></returns>
        List<ProductCategoryDto> GetProductCategories();

        /// <summary>
        /// 创建一个新的种类
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        int CreateProductCategory(ProductCategoryDto productCategory);

        /// <summary>
        ///     修改一个种类
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        int UpdateProductCategory(ProductCategoryDto productCategoryDto);

        /// <summary>
        /// 获取所有种类的树形结构树
        /// </summary>
        /// <returns></returns>
        ResponseBase<List<ProductCategoryDto>> GetProductcategory();

        /// <summary>
        /// 获取产品分类中最大的CategoryNumber
        /// </summary>
        /// <returns></returns>
        int GetMaxCategoryNumberInProductCategory();

        #endregion

        #region 商品属性

        /// <summary>
        /// 获取产品属性中最大的AttributeNumber
        /// </summary>
        /// <returns></returns>
        int GetMaxAttributeNumberInProductAttribute();

        /// <summary>
        /// 获取产品属性值中最大的AttributeValueNumber
        /// </summary>
        /// <returns></returns>
        int GetMaxAttributeValueNumberInAttributeValue();

        /// <summary>
        ///     创建一个新的属性
        /// </summary>
        /// <param name="productAttributeDto"></param>
        /// <returns></returns>
        int CreateProductAttribute(ProductAttributeDto productAttributeDto);

        /// <summary>
        /// 获取属性值信息
        /// </summary>
        /// <param name="id">属性值ID</param>
        /// <returns></returns>
        AttributeValuesDto GetAttributeValueByID(Guid id);

        /// <summary>
        ///     创建一个新的属性值
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        int CreateAttributeValue(AttributeValuesDto attributeValuesDto);

        /// <summary>
        ///     编辑属性值
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        int EditProductAttributeValue(AttributeValuesDto dto);

        /// <summary>
        ///     编辑属性
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        int EditProductAttribute(ProductAttributeDto dto);

        /// <summary>
        ///     逻辑删除属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteProductAttribute(Guid id);

        /// <summary>
        ///     逻辑删除属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int DeleteProductAttributeValue(Guid id);

        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <returns></returns>
        ProductAttributeDto GetProductAttributeByID(Guid attributeId);

        /// <summary>
        /// 获取属性相关信息
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns></returns>
        List<ProductAttributeDto> GetProductAttributeByCategory(Guid categoryId);

        /// <summary>
        /// 校验属性是否存在
        /// </summary>
        /// <param name="attName"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        bool IsExistProductAttribute(string attName, Guid categoryId);

        /// <summary>
        /// 校验属性值是否存在
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseBase<bool> IsExistAttributeValue(AttributeValuesDto dto);

        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        SearchResponseBase<List<ProductAttributeDto>> SearchProductAttribute(ProductAttributeSearchDto searchDto);

        /// <summary>
        ///     创建一个新的商品
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        int CreateProdut(ProductDto productDto);

        /// <summary>
        /// 更新产品第二步
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        int UpdateProductStepTwo(ProductDto productDto);

        /// <summary>
        /// 加载图片信息
        /// </summary>
        List<GoodsPictureDto> LoadPicturesByGoodsId(Guid goodsId);

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="goodsDto"></param>
        /// <returns></returns>
        int CreatePictures(GoodsDto goodsDto);

        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<GoodsDto> GetGoods(GoodsSearchDto search, out int total);

        /// <summary>
        /// 创建Goods对应价格
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        int CreateGoodsPrices(GoodsDto goodsDto);

        /// <summary>
        /// 构造Goods
        /// </summary>
        /// <param name="goods"></param>
        /// <returns></returns>
        int BiuldGoods(GoodsDto goods);

        /// <summary>
        /// Get GoodsNumbers
        /// </summary>
        /// <returns></returns>
        List<int> GetGoodsNumbers();

        /// <summary>
        /// 存储goods的属性及值
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        int CreateGoodsAttributeValues(GoodsDto goodsDto);

        /// <summary>
        ///     创建一个新的商品选定属性
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns></returns>
        int CreateProductSelectedAttibutes(ProductSelectedAttibutesDto dto);

        /// <summary>
        /// 物理删除产品所选属性
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        int DeleteProductSelectedAttributevVlues(Guid ProductId);

        /// <summary>
        /// 更新产品
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        //int UpdateProduct(Guid productId, List<MySqlParameter> parameters);

        /// <summary>
        /// 物理删除配送信息 by Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        int DeleteDeliveryinfoByProductId(Guid productId);

        /// <summary>
        /// 存储临时goods的销售属性及值
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        int CreateTempGoodsAttributeValues(TempGoodsDto tempGoodsDto);

        /// <summary>
        /// 更新产品状态字段
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        int UpdateProductStatus(Guid productId, ProductStatusCode ProductStatus);

        /// <summary>
        /// 获取属性配置表
        /// </summary>
        /// <param name="tempGoodsId"></param>
        /// <returns></returns>
        List<SelectedAttibutesDto> GetAttributeValues(SelectedAttibutesDto search);

        /// <summary>
        /// 物理删除属性配置表
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        int DeleteAttributevVlues(SelectedAttibutesDto search);

        /// <summary>
        /// 存储一个临时Goods
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        int CreateTempGoods(TempGoodsDto tempGoodsDto);

        /// <summary>
        /// 创建图片
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        int CreatePictures(TempGoodsDto tempGoodsDto);

        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        List<ProductDto> GetProducts(ProductSearchDto search, out int total);

        /// <summary>
        /// 物理删除Pirces by Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        int DeletePircesByProductId(Guid productId);

        /// <summary>
        /// 物理删除Pirces by TempGoods
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        int DeletePircesByTempGoodsId(Guid tempGoodsId);

        /// <summary>
        /// 物理删除Pictures by TempGoods
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        int DeletePicturesByTempGoodsId(Guid tempGoodsId);

        /// <summary>
        /// 物理删除TempGoods
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        int DeleteTempGoodsByProductId(Guid productId);

        /// <summary>
        /// 获取商品个数
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        int GetProductNumberByFilter(ProductSearchDto search);

        /// <summary>
        /// 获取商品个数
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        int GetGoodsNumberByFilter(GoodsSearchDto search);

        /// <summary>
        /// 删除多个商品
        /// </summary>
        /// <param name="ids">商品Id集合</param>
        /// <returns></returns>
        int DeleteProductById(List<Guid> ids);

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="ids">商品Id集合</param>
        /// <param name="initStatus">初始状态</param>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="submintUser">更新用户</param>
        /// <returns></returns>
        int SetProductById(List<Guid> ids,
            ProductStatusCode? initStatus,
            ProductStatusCode targetStatus,
            Guid submintUser);

        /// <summary>
        /// 更新Goods商品状态
        /// </summary>
        /// <param name="ids">Goods商品Id集合</param>
        /// <param name="initStatus">初始状态</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns></returns>
        int SetGoodsStatusById(List<Guid> ids, ProductStatusCode? initStatus, ProductStatusCode targetStatus);

        /// <summary>
        /// 加载临时Goods信息
        /// </summary>
        List<TempGoodsDto> LoadTempGoodsByProductId(Guid productId);

        /// <summary>
        /// 加载图片信息
        /// </summary>
        List<GoodsPictureDto> LoadPicturesByTempGoodsId(Guid tempGoodsId);

        /// <summary>
        /// 加载TempGoods价格信息
        /// </summary>
        List<GoodsPriceDto> LoadPircesByTempGoodsId(Guid tempGoodsId);

        /// <summary>
        /// 加载Product价格信息
        /// </summary>
        List<GoodsPriceDto> LoadPircesByProductId(Guid productId);

        /// <summary>
        /// 加载Goods价格信息
        /// </summary>
        List<GoodsPriceDto> LoadPircesByGoodsId(Guid goodsId);


        /// <summary>
        /// 创建TempGoods对应价格
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        int CreateTempGoodsPrices(TempGoodsDto tempGoodsDto);

        /// <summary>
        /// 创建Product对应价格
        /// </summary>
        /// <param name="tempGoodsDto"></param>
        /// <returns></returns>
        int CreatePricesProduct(ProductDto productDto);

        /// <summary>
        /// 更新产品交易信息
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        int UpdateProductTradeInfo(ProductDto productDto);


        /// <summary>
        /// 更新配送说明
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        int UpdateProductDeliveryDesc(ProductDto productDto);

        /// <summary>
        /// 更新产品SetpFive
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        int UpdateProductStepFive(ProductDto productDto);

        /// <summary>
        /// 处理配送信息
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        int ProcessProductDelivery(ProductDto productDto);

        /// <summary>
        /// 获取Product配送信息
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        List<DeliveryInfoDto> GetDeliveryInfo(Guid productId);

        /// <summary>
        /// 加载Product选择属性信息
        /// </summary>
        List<ProductSelectedAttibutesDto> LoadSelectedAttibutesByProductId(Guid productId);

        /// <summary>
        /// 更新产品未通过原因
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        int UpdateProductNoPassReason(Guid productId, string reason);
     

        #endregion
    }
}
