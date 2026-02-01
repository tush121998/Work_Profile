using System;
using Shop.Web.Models;

namespace Shop.Web.Service.IService;

public interface ICartService
{
    Task<ResponseDto?> GetCartByUserIdAsync(string userId);
    Task<ResponseDto?> UpsertAsync(CartDto cartDto);
    Task<ResponseDto?> RemoveFromCarAsync(int cartDetailsId);
    Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);
     Task<ResponseDto?> RemoveCouponAsync(CartDto cartDto);
}
