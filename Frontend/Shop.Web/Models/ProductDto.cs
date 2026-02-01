using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Shop.Web.Models;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string CategoryName { get; set; }
    public string ImageUrl { get; set; }

    [Range(1, 100, ErrorMessage = "Please enter a value between 1 and 100")]
    public int Count { get; set; } = 1;
}
