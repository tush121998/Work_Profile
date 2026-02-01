using System;
using Shop.Services.CartAPI.Models.Dto;

namespace Shop.Services.CartAPI.Service.IService;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetProducts();
}
