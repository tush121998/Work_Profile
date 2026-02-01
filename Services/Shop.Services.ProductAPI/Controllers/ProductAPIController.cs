using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Services.ProductAPI.Data;
using Shop.Services.ProductAPI.Models;
using Shop.Services.ProductAPI.Models.Dto;

namespace Shop.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductAPIController(AppDbContext context)
        {
            _context = context;
        }

        // Define your API methods here
        [HttpGet("GetAll")]
        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();
            return Ok(new ResponseDto
            {
                Result = products,
                IsSuccess = true,
                Message = "Products retrieved successfully."
            });
        }

        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetProductById(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Product not found."
                });
            }

            return Ok(new ResponseDto
            {
                Result = new ProductDto
                {
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    CategoryName = product.CategoryName,
                    ImageUrl = product.ImageUrl
                },
                IsSuccess = true,
                Message = "Product retrieved successfully."
            });
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateProduct([FromBody] ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Description = productDto.Description,
                CategoryName = productDto.CategoryName,
                ImageUrl = productDto.ImageUrl
            };
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(new ResponseDto
            {
                Result = new ProductDto
                {
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description
                },
                IsSuccess = true,
                Message = "Product created successfully."
            });
        }

        [HttpPost("Update")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateProduct([FromBody] ProductDto productDto)
        {
            var product = _context.Products.Find(productDto.ProductId);
            if (product == null)
            {
                return NotFound(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Product not found."
                });
            }
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            _context.SaveChanges();

            return Ok(new ResponseDto
            {
                Result = productDto,
                IsSuccess = true,
                Message = "Product updated successfully."
            });
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound(new ResponseDto
                {
                    IsSuccess = false,
                    Message = "Product not found."
                });
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok(new ResponseDto
            {
                Result = null,
                IsSuccess = true,
                Message = "Product deleted successfully."
            });
        }
    }
}
