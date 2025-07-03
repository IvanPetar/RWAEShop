using System.ComponentModel.DataAnnotations.Schema;

namespace RWAEShopWebApi.DTOs
{
    public class ProductResponseDto
    {
        public string Name { get; set; }

        public string ProductDescription { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public List<string> CountryNames { get; set; } = new();
    }
}
