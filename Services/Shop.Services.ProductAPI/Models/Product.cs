using System;
using System.ComponentModel.DataAnnotations;

namespace Shop.Services.ProductAPI.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public double Price { get; set; }
    public string CategoryName { get; set; }
    public string ImageUrl { get; set; }
}
