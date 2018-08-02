using System;
using System.Collections.Generic;
using MXJ.Projects.DataTransferObjects;
using MXJ.Projects.Domain.Enums;

namespace MXJ.Projects.IBussinessServices
{
    /// <summary>
    ///     商品业务层
    /// </summary>
    public interface IGoodsBussinessService : IBaseBussinessService
    {
        #region 商品品类
        /// <summary>
        /// 获取所有种类
        /// </summary>
        /// <returns></returns>
        ResponseBase<List<ProductCategoryDto>> GetProductCategories(ProductCategorySearchDto search);

        /// <summary>
        /// 获取种类
        /// </summary>
        /// <returns></returns>
        ResponseBase<ProductCategoryDto> GetProductCategoryById(Guid categoryId);

        /// <summary>
        /// 根据编号获取种类
        /// </summary>
        /// <returns></returns>
        ResponseBase<ProductCategoryDto> GetProductCategoryByNumber(int number);

        /// <summary>
        ///     创建一个新的种类
        /// </summary>
        /// <param name="productCategoryDto"></param>
        /// <returns></returns>
        ResponseBase<bool> CreateProductCategory(ProductCategoryDto productCategoryDto);

        /// <summary>
        ///     编辑商品种类
        /// </summary>
        /// <param name="productCategoryDto"></param>
        /// <returns></returns>
        ResponseBase<bool> EditProductCategory(ProductCategoryDto productCategoryDto);
        
        /// <summary>
        /// 获取所有种类的树形结构树
        /// </summary>
        /// <returns></returns>
        ResponseBase<List<ProductCategoryDto>> GetProductcategory();
        #endregion

        #region 商品属性
        /// <summary>
        ///     创建一个新的属性
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseBase<bool> CreateProductAttribute(ProductAttributeDto dto);

        /// <summary>
        /// 获取属性值信息
        /// </summary>
        /// <param name="id">属性值ID</param>
        /// <returns></returns>
        ResponseBase<AttributeValuesDto> GetAttributeValueByID(Guid id);
        
        /// <summary>
        ///     编辑属性
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseBase<bool> EditProductAttribute(ProductAttributeDto dto);

        /// <summary>
        ///     逻辑删除属性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResponseBase<bool> DeleteProductAttribute(Guid id);
        
        /// <summary>
        /// 获取属性信息
        /// </summary>
        /// <param name="attributeId">属性ID</param>
        /// <returns></returns>
        ResponseBase<ProductAttributeDto> GetProductAttributeByID(Guid attributeId);

        /// <summary>
        /// 获取属性相关信息
        /// </summary>
        /// <param name="categoryId">类别ID</param>
        /// <returns></returns>
        ResponseBase<List<ProductAttributeDto>> GetProductAttributeByCategory(Guid categoryId);

        /// <summary>
        /// 获取属性列表
        /// </summary>
        /// <param name="searchDto"></param>
        /// <returns></returns>
        SearchResponseBase<List<ProductAttributeDto>> SearchProductAttribute(ProductAttributeSearchDto searchDto);

        #endregion

        #region 属性值
        /// <summary>
        ///     创建一个新的属性值
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseBase<bool> CreateProductAttributeValue(AttributeValuesDto dto);

        /// <summary>
        ///     编辑属性值
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ResponseBase<bool> EditProductAttributeValue(AttributeValuesDto dto);

        /// <summary>
        ///     逻辑删除属性值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ResponseBase<bool> DeleteProductAttributeValue(Guid id);
        #endregion

        #region 产品
        /// <summary>
        /// 查询Goods
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        ResponseBase<List<GoodsDto>> GetGoods(GoodsSearchDto search, out int total);

        /// <summary>
        /// 获取单个Goods
        /// </summary>
        /// <param name="goodsNumber"></param>
        /// <returns></returns>
        ResponseBase<GoodsDto> GetGoods(int goodsNumber);

        /// <summary>
        /// 创建一个新的商品
        /// </summary>
        /// <param name="productCategoryDto"></param>
        /// <returns></returns>
        ResponseBase<ProductDto> CreateProduct(ProductDto productDto);

        /// <summary>
        /// 存储TempGoods
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        ResponseBase<ProductDto> CreateTempGoods(ProductDto productDto);

        /// <summary>
        /// 获取单个商品
        /// </summary>
        /// <param name="id">ProductId</param>
        /// <param name="isLoadTempGoods">是否加TempGoods信息</param>
        /// <param name="isLoadPictures">是否加载图片信息</param>
        /// <param name="isLoadPirces">是否加载价格信息</param>
        /// <param name="isLoadDeliveryInfo">是否加载配送信息</param>
        /// <returns></returns>
        ResponseBase<ProductDto> GetProductById(Guid id, bool isLoadTempGoods, bool isLoadPictures, bool isLoadPirces, bool isLoadDeliveryInfo,bool IsLoadSelectedAttibutes = false);
        
        /// <summary>
        /// 更新产品状态字段
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        ResponseBase<bool> UpdateProductStatus(Guid productId, ProductStatusCode ProductStatus);

        /// <summary>
        /// 查询商品
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        SearchResponseBase<List<ProductDto>> GetProducts(ProductSearchDto search);

        /// <summary>
        /// 获取产品个数
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        ResponseBase<int> GetProductNumberByFilter(ProductSearchDto search);

        /// <summary>
        /// 获取商品个数
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        ResponseBase<int> GetGoodsNumberByFilter(GoodsSearchDto search);

        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="ids">商品Id集合</param>
        /// <returns></returns>
        ResponseBase<bool> DeleteProductById(List<Guid> ids);

        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="ids">商品Id集合</param>
        /// <param name="initStatus">初始状态</param>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="submintUser">更新用户</param>
        /// <returns></returns>
        ResponseBase<bool> SetProductById(List<Guid> ids,ProductStatusCode? initStatus,
            ProductStatusCode targetStatus,Guid submintUser);

        /// <summary>
        /// 更新Goods商品状态
        /// </summary>
        /// <param name="ids">Goods商品Id集合</param>
        /// <param name="initStatus">初始状态</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns></returns>
        ResponseBase<bool> SetGoodsStatusById(List<Guid> ids, ProductStatusCode? initStatus, ProductStatusCode targetStatus);

        /// <summary>
        /// 第四步操作更新产品
        /// </summary>
        /// <param name="productDto"></param>
        /// <returns></returns>
        ResponseBase<Guid> UpdateProductSetpFour(ProductDto productDto);

        /// <summary>
        /// 处理配送信息
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns></returns>
        ResponseBase<ProductDto> ProcessProductDelivery(ProductDto productDto);

        /// <summary>
        /// 获取Product配送信息
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns></returns>
        ResponseBase<ProductDto> GetDeliveryInfo(Guid productId);

        /// <summary>
        /// 更新产品详细
        /// </summary>
        /// <param name="id">商品Id</param>
        /// <returns></returns>
        ResponseBase<ProductDto> UpdateProductStepFive(ProductDto productDto);

        /// <summary>
        /// 商品未通过
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ResponseBase<ProductDto> ProductNoPass(Guid productId, string reason);

        /// <summary>
        /// 商品上架
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ResponseBase<ProductDto> ProductToGoods(Guid productId);

        #endregion

    }
}