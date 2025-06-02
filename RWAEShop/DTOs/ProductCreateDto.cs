using System.ComponentModel.DataAnnotations;

namespace RWAEShop.DTOs
{
    public class ProductCreateDto
    {

        [StringLength(100)]
        public string Name { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }

        public List<int> CountryId { get; set; } = new();


    }
}
