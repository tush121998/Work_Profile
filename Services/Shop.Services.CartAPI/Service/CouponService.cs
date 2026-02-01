using System;
using Newtonsoft.Json;
using Shop.Services.CartAPI.Models.Dto;
using Shop.Services.CartAPI.Service.IService;

namespace Shop.Services.CartAPI.Service;

public class CouponService : ICouponService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CouponService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<CouponDto> GetCoupon(string couponCode)
    {
       var client = _httpClientFactory.CreateClient("Coupon");
        var response = await client.GetAsync($"/api/CouponAPI/GetByCode/{couponCode}");
        var apiContent = await response.Content.ReadAsStringAsync();
        var res = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

        if(res.IsSuccess)
        {
            return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(res.Result));
        }
        return new CouponDto();
    }
}
