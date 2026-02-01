using System;
using Shop.Web.Models;
using Shop.Web.Service.IService;
using Shop.Web.Utility;
using static Shop.Web.Utility.SD;

namespace Shop.Web.Service;
    
public class CouponService : ICouponService
{
    private readonly IBaseService _baseService;

    public CouponService(IBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<ResponseDto> GetCouponByCodeAsync(string couponCode)
    {
        var response = await _baseService.SendAsync<CouponDto>(new RequestDto
        {
            ApiType = APITYPE.GET,
            Url = SD.CouponApiBase +$"/api/couponapi/{couponCode}"
        });
        return response;
    }

    public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
    {
        var response = await _baseService.SendAsync<CouponDto>(new RequestDto
        {
            ApiType = APITYPE.POST,
            Url = SD.CouponApiBase + "/api/couponapi/create",
            Data = couponDto
        });
        return response;
    }

    public async Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
    {
        var response = await _baseService.SendAsync<CouponDto>(new RequestDto
        {
            ApiType = APITYPE.PUT,
            Url = SD.CouponApiBase + $"/api/coupon/{couponDto.CouponId}",
            Data = couponDto
        });
        return response;
    }

    public async Task<ResponseDto> DeleteCouponAsync(int couponId)
    {
        var response = await _baseService.SendAsync<CouponDto>(new RequestDto
        {
            ApiType = APITYPE.DELETE,
            Url = SD.CouponApiBase +$"/api/couponapi/{couponId}"
        });
        return response;
    }

    public async Task<ResponseDto> GetAllCouponsAsync()
    {
        var response = await _baseService.SendAsync<CouponDto>(new RequestDto
        {
            ApiType = APITYPE.GET,
            Url = SD.CouponApiBase + "/api/couponapi/GetAll"
        });
        return response;
    }

    public async Task<ResponseDto> GetCouponByIdAsync(int couponId)
    {
        var response = await _baseService.SendAsync<CouponDto>(new RequestDto
        {
            ApiType = APITYPE.GET,
            Url = SD.CouponApiBase +$"/api/couponapi/{couponId}"
        });
        return response;
    }
}
