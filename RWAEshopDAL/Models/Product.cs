using System;
using System.Collections.Generic;

namespace RWAEshopDAL.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public string Name { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? ImageUrl { get; set; }

    public int? CategoryId { get; set; }

    public virtual ProductCategory? Category { get; set; }

    public virtual ICollection<CountryProduct> CountryProducts { get; set; } = new List<CountryProduct>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
