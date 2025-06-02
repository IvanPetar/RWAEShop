using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RWAEshopDAL.Models;

[Table("Country")]
public partial class Country
{
    [Key]
    public int IdCountry { get; set; }

    [StringLength(70)]
    public string Name { get; set; } = null!;

    [InverseProperty("Country")]
    public virtual ICollection<CountryProduct> CountryProducts { get; set; } = new List<CountryProduct>();
}
