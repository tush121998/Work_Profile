using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shop.Web.Models;
using Shop.Web.Service.IService;

namespace Shop.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICartService _cartService;

    public HomeController(IProductService productService, ICartService cartService)
    {
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        List<ProductDto> products = new();
        var response = await _productService.GetAllProductsAsync();
        if(response != null && response.IsSuccess)
        {
            products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["Error"] = response?.Message;
        }
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> ProductDetails(int productId)
    {
        ProductDto product = new();
        var response = await _productService.GetProductByIdAsync(productId);
        if(response != null && response.IsSuccess)
        {
            product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
        }
        else
        {
            TempData["Error"] = response?.Message;
        }
        return View(product);
    }

    [Authorize]
    [HttpPost]
    [ActionName("ProductDetails")]
    public async Task<IActionResult> ProductDetails(ProductDto productDto)
    {
        CartDto cartDto = new CartDto()
        {
            CartHeader = new CartHeaderDto
            {
                UserId = User.Claims.Where(x => x.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value
            }
        };
        CartDetailsDto cartDetailsDto = new CartDetailsDto
        {
            Count = productDto.Count,
            ProductId = productDto.ProductId

        };
        List<CartDetailsDto> cartDetailsDtos = new List<CartDetailsDto>
        {
            cartDetailsDto
        };
        cartDto.CartDetails = cartDetailsDtos;

        var response = await _cartService.UpsertAsync(cartDto);
        if(response != null && response.IsSuccess)
        {
            TempData["success"] = "Item has been added to the Shopping cart.";
           RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["Error"] = response?.Message;
        }
        return View(productDto);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
