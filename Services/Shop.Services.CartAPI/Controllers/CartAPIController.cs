using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Shop.Services.CartAPI.Data;
using Shop.Services.CartAPI.Models;
using Shop.Services.CartAPI.Models.Dto;
using Shop.Services.CartAPI.Service.IService;

namespace Shop.Services.CartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        public CartAPIController(IMapper mapper, AppDbContext context, IProductService productService, ICouponService couponService)
        {
            _mapper = mapper;
            _context = context;
            _productService = productService;
            _couponService = couponService;
        }

        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private ResponseDto _response = new ();

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)      
        {
            try
            {
                CartDto cartDto = new (){
                    CartHeader = _mapper.Map<CartHeaderDto>(_context.CartHeaders.First(u => u.UserId == userId))
                };
                cartDto.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_context.CartDetails
                .Where(u => u.CartHeaderId == cartDto.CartHeader.CartHeaderId));

                IEnumerable<ProductDto> productDtos = await _productService.GetProducts();
                foreach(var item in cartDto.CartDetails)
                {
                    item.Product = productDtos.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cartDto.CartHeader.CartTotal += item.Count * item.Product.Price;
                }

                ////apply coupon if any
                if(!string.IsNullOrEmpty(cartDto.CartHeader.CouponCode))
                {
                    CouponDto couponDto = await _couponService.GetCoupon(cartDto.CartHeader.CouponCode);
                    if(couponDto != null && cartDto.CartHeader.CartTotal > couponDto.MinAmount)
                    {
                        cartDto.CartHeader.CartTotal -= couponDto.DiscountAmount;
                        cartDto.CartHeader.Discount = couponDto.DiscountAmount;
                    }
                }
                _response.Result = cartDto;
            }                          
            catch (Exception ex)
            {
                _response = new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            return _response;
        }


        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon(CartDto cartDto)
        {
            try
            {
                var cartFromDB = _context.CartHeaders.First(x => x.UserId == cartDto.CartHeader.UserId);
                cartFromDB.CouponCode = cartDto.CartHeader.CouponCode;
                _context.CartHeaders.Update(cartFromDB);
                await _context.SaveChangesAsync();
                _response.Result = true;

            }
            catch(Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }        

        [HttpPost("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon(CartDto cartDto)
        {
            try
            {
                var cartFromDB = _context.CartHeaders.First(x => x.UserId == cartDto.CartHeader.UserId);
                cartFromDB.CouponCode = "";
                _context.CartHeaders.Update(cartFromDB);
                await _context.SaveChangesAsync();
                _response.Result = true;

            }
            catch(Exception ex)
            {
                _response.Message = ex.Message;
                _response.IsSuccess = false;
            }
            return _response;
        }   

        [HttpPost("UpsertCart")]
        public async Task<ResponseDto> UpsertCartItem(CartDto cartDto)      
        {
            try
            {
                var cartHeaderFromDb = _context.CartHeaders.AsNoTracking()
                    .FirstOrDefault(c => c.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDb == null)
                {
                    //create cart header
                    _context.CartHeaders.Add(_mapper.Map<Shop.Services.CartAPI.Models.CartHeader>(cartDto.CartHeader));
                    await _context.SaveChangesAsync();
                    cartDto.CartDetails.First().CartHeaderId = cartDto.CartHeader.CartHeaderId;
                    //create cart details
                    _context.CartDetails.Add(_mapper.Map<Shop.Services.CartAPI.Models.CartDetails>(cartDto.CartDetails.First()));
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var cartDetailsFromDb = _context.CartDetails.AsNoTracking()
                        .FirstOrDefault(cd => cd.ProductId == cartDto.CartDetails.First().ProductId
                        && cd.CartHeaderId == cartHeaderFromDb.CartHeaderId);
                    if (cartDetailsFromDb == null)
                    {
                        // Add new cart detail
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _context.CartDetails.Add(_mapper.Map<Shop.Services.CartAPI.Models.CartDetails>(cartDto.CartDetails.First()));
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        // Update existing cart detail
                        cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _context.CartDetails.Update(cartDetailsFromDb);  
                        await _context.SaveChangesAsync(); 
                    }
                }
                // Sample action method to illustrate structure
                _response = new ResponseDto
                {
                    IsSuccess = true,
                    Message = "Cart item upserted successfully",
                    Result = cartDto
                };
            }                          
            catch (Exception ex)
            {
                _response = new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCartItem(int carDetailsId)      
        {
            try
            {
                CartDetails cartDetails = _context.CartDetails.First(u=>u.CartDetailsId == carDetailsId);
                int totalCountofCartItem = _context.CartDetails.Where(x=> x.CartHeaderId == cartDetails.CartHeaderId).Count();
                _context.CartDetails.Remove(cartDetails);
                if(totalCountofCartItem == 1)
                {
                    var carHeaderToRemove = await _context.CartHeaders.FirstOrDefaultAsync(x => x.CartHeaderId == cartDetails.CartHeaderId);
                    _context.CartHeaders.Remove(carHeaderToRemove);
                }
                await _context.SaveChangesAsync();
                _response.IsSuccess = true;
                _response.Message = "Removed Item from Cart Succesfully.";
            }                          
            catch (Exception ex)
            {
                _response = new ResponseDto
                {
                    IsSuccess = false,
                    Message = ex.Message,
                };
            }
            return _response;
        }

        
    }
}
