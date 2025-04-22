using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RWAEShop.Models;

[Table("CountryProduct")]
public partial class CountryProduct
{
    [Key]
    public int IdCountryProduct { get; set; }

    public int CountryId { get; set; }

    public int ProductId { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("CountryProducts")]
    public virtual Country Country { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("CountryProducts")]
    public virtual Product Product { get; set; } = null!;
}
