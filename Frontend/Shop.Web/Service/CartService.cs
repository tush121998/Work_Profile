using System;
using Shop.Web.Models;
using Shop.Web.Service.IService;
using Shop.Web.Utility;

namespace Shop.Web.Service;

public class CartService : ICartService
{

    private readonly IBaseService _baseService;

    public CartService(IBaseService baseService) 
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
    {
        return await _baseService.SendAsync<ResponseDto>(new RequestDto
       {
          ApiType = SD.APITYPE.POST,
          Url = SD.CartApiBase + "/api/CartAPI/ApplyCoupon",
          Data = cartDto
       });
    }

    public async Task<ResponseDto?> GetCartByUserIdAsync(string userId)
    {
       return await _baseService.SendAsync<ResponseDto>(new RequestDto
       {
          ApiType = SD.APITYPE.GET,
          Url = SD.CartApiBase + "/api/CartAPI/GetCart/" + userId
       });
    }

    public async Task<ResponseDto?> RemoveFromCarAsync(int cartDetailsId)
    {
         return await _baseService.SendAsync<ResponseDto>(new RequestDto
       {
          ApiType = SD.APITYPE.POST,
          Url = SD.CartApiBase + "/api/CartAPI/RemoveCart/",
          Data = cartDetailsId
       });
    }

    public async Task<ResponseDto?> UpsertAsync(CartDto cartDto)
    {
         return await _baseService.SendAsync<ResponseDto>(new RequestDto
       {
          ApiType = SD.APITYPE.POST,
          Url = SD.CartApiBase + "/api/CartAPI/UpsertCart",
          Data = cartDto
       });
    }

     public async Task<ResponseDto?> RemoveCouponAsync(CartDto cartDto)
    {
         return await _baseService.SendAsync<ResponseDto>(new RequestDto
       {
          ApiType = SD.APITYPE.POST,
          Url = SD.CartApiBase + "/api/CartAPI/RemoveCoupon",
          Data = cartDto
       });
    }
}
