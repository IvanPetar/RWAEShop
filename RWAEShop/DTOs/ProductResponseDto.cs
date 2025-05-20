using System.ComponentModel.DataAnnotations.Schema;

namespace RWAEShop.DTOs
{
    public class ProductResponseDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}
