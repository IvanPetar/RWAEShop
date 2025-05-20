using System.ComponentModel.DataAnnotations;

namespace RWAEShop.DTOs
{
    public class ProductCreateDto
    {
        public int CategoryId { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }


    }
}
