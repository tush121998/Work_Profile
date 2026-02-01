using System;
using Shop.Services.CartAPI.Models.Dto;

namespace Shop.Services.CartAPI.Service.IService;

public interface ICouponService
{
    Task<CouponDto> GetCoupon(string couponCode);
}
