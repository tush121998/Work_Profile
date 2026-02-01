using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.IService;

namespace Shop.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? couponDtos = null;
            ResponseDto coupons = await _couponService.GetAllCouponsAsync();
            if (coupons != null && coupons.IsSuccess)
            {
                couponDtos = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(coupons.Result));
            }
            else
            {
                TempData["Error"] = coupons?.Message;
            }
            return View(couponDtos);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDto? response = await _couponService.DeleteCouponAsync(couponId);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CouponIndex));
            }
            return View();
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto couponDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _couponService.CreateCouponAsync(couponDto);
                if (response != null && response.IsSuccess)
                {
                    TempData["Success"] = "Coupon created successfully";
                    return RedirectToAction(nameof(CouponIndex));
                } 
                else
                {
                    TempData["Error"] = response?.Message;
                }
            }
            return View();
        }

        public async Task<IActionResult> DeleteCoupon(int couponId)
        {
            if (couponId > 0)
            {
                ResponseDto? response = await _couponService.DeleteCouponAsync(couponId);
                if (response != null && response.IsSuccess)
                {
                    TempData["Success"] = "Coupon deleted successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                 else
                {
                    TempData["Error"] = response?.Message;
                }
            }
            return View();
        }
        
    }
}
