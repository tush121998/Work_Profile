using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Coupon.API.Data;
using Services.Coupon.API.Models.Dto;

namespace Services.Coupon.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CouponAPIController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ResponseDto> GetAll()
        {
            try
            {
                var coupons = await _context.Coupons.ToListAsync();
                return new ResponseDto
                {
                    IsSuccess = true,
                    Result = _mapper.Map<List<CouponDto>>(coupons)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = $"Internal server error: {ex.Message}"
                };
            }
        }
        [HttpGet("GetById/{id}")] 
        [Authorize(Roles = "Admin,User")]  
        public async Task<ResponseDto> GetById(int id)
        {
            try
            {
                var coupon = await _context.Coupons.FindAsync(id);
                if (coupon == null)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Coupon not found"
                    };
                }
                return new ResponseDto
                {
                    IsSuccess = true,
                    Result = _mapper.Map<CouponDto>(coupon)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = $"Internal server error: {ex.Message}"
                };
            }
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> Create([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Models.Coupon>(couponDto);
                _context.Coupons.Add(coupon);
                await _context.SaveChangesAsync();
                return new ResponseDto
                {
                    IsSuccess = true,
                    Result = _mapper.Map<CouponDto>(coupon)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = $"Internal server error: {ex.Message}"
                };
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> Update(int id, [FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = await _context.Coupons.FindAsync(id);
                if (coupon == null)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Coupon not found"
                    };
                }
                _mapper.Map(couponDto, coupon);
                _context.Coupons.Update(coupon);
                await _context.SaveChangesAsync();
                return new ResponseDto
                {
                    IsSuccess = true,
                    Result = _mapper.Map<CouponDto>(coupon)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = $"Internal server error: {ex.Message}"
                };
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ResponseDto> Delete(int id)
        {
            try
            {
                var coupon = await _context.Coupons.FindAsync(id);
                if (coupon == null)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Coupon not found"
                    };
                }
                _context.Coupons.Remove(coupon);
                await _context.SaveChangesAsync();
                return new ResponseDto
                {
                    IsSuccess = true,
                    Message = "Coupon deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = $"Internal server error: {ex.Message}"
                };
            }
        }

        [HttpGet("GetByCode/{code}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ResponseDto> GetByCode(string code)
        {
            try
            {
                var coupon = await _context.Coupons
                    .FirstOrDefaultAsync(c => c.CouponCode == code);
                if (coupon == null)
                {
                    return new ResponseDto
                    {
                        IsSuccess = false,
                        Message = "Coupon not found"
                    };
                }
                return new ResponseDto
                {
                    IsSuccess = true,
                    Result = _mapper.Map<CouponDto>(coupon)
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    IsSuccess = false,
                    Message = $"Internal server error: {ex.Message}"
                };
            }
        }
    }
}
