using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RWAEshopDAL.Models;

[Table("Product")]
public partial class Product
{
    [Key]
    public int IdProduct { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(4000)]
    public string ProductDescription { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? ImageUrl { get; set; }

    public int? CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual ProductCategory? Category { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<CountryProduct> CountryProducts { get; set; } = new List<CountryProduct>();

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
