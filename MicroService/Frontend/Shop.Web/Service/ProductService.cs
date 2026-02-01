using System;
using Shop.Web.Models;
using Shop.Web.Service.IService;
using Shop.Web.Utility;
using static Shop.Web.Utility.SD;

namespace Shop.Web.Service;

public class ProductService : IProductService
{
    private readonly IBaseService _baseService;

    public ProductService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto> GetAllProductsAsync()
    {
        var response = await _baseService.SendAsync<IEnumerable<ProductDto>>(new RequestDto
        {
            ApiType = APITYPE.GET,
            Url = SD.ProductApiBase + $"/api/ProductAPI/GetAll"
        });
        return response;
    }

    public async Task<ResponseDto> GetProductByIdAsync(int productId)
    {
        var response = await _baseService.SendAsync<ProductDto>(new RequestDto
        {
            ApiType = APITYPE.GET,
            Url = SD.ProductApiBase + $"/api/ProductAPI/GetById/{productId}"
        });
        return response;
    }

    public async Task<ResponseDto> CreateProductAsync(ProductDto productDto)
    {
        var response = await _baseService.SendAsync<ProductDto>(new RequestDto
        {
            ApiType = APITYPE.POST,
            Url = SD.ProductApiBase + $"/api/ProductAPI/Create",
            Data = productDto
        });
        return response;    
    }

    public async Task<ResponseDto> UpdateProductAsync(ProductDto productDto)
    {
        var response = await _baseService.SendAsync<ProductDto>(new RequestDto
        {
            ApiType = APITYPE.POST,
            Url = SD.ProductApiBase + $"/api/ProductAPI/Update",
            Data = productDto
        });
        return response;
    }

    public async Task<ResponseDto> DeleteProductAsync(int productId)
    {
        var response = await _baseService.SendAsync<object>(new RequestDto
        {
            ApiType = APITYPE.DELETE,
            Url = SD.ProductApiBase + $"/api/ProductAPI/Delete/{productId}"
        });
        return response;
    }
}
