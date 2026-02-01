using System;
using Shop.Web.Models;

namespace Shop.Web.Service.IService;

public interface IProductService
{
    Task<ResponseDto> GetAllProductsAsync();
    Task<ResponseDto> GetProductByIdAsync(int productId);
    Task<ResponseDto> CreateProductAsync(ProductDto productDto);
    Task<ResponseDto> UpdateProductAsync(ProductDto productDto);
    Task<ResponseDto> DeleteProductAsync(int productId);
}
