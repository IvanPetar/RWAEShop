using System.ComponentModel.DataAnnotations;

namespace RWAEShop.DTOs
{
    public class CategoryResponseDto
    {
        public int IdCategory { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
