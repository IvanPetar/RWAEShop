using System;
using System.Collections.Generic;

namespace RWAEshopDAL.Models;

public partial class ProductCategory
{
    public int IdCategory { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
