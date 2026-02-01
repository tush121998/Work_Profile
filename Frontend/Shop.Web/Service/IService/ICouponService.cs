using System;
using Shop.Web.Models;

namespace Shop.Web.Service.IService;

public interface ICouponService
{
    Task<ResponseDto> GetCouponByCodeAsync(string couponCode);
    Task<ResponseDto> CreateCouponAsync(CouponDto couponDto);
    Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto);
    Task<ResponseDto> DeleteCouponAsync(int couponId);
    Task<ResponseDto> GetAllCouponsAsync();
    Task<ResponseDto> GetCouponByIdAsync(int couponId);
}
