using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RWAEShopWebApi.DTOs
{
    public class ProductUpdateDto
    {
        public int? CategoryId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }
    }
}
