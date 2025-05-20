using System.ComponentModel.DataAnnotations;

namespace RWAEShop.DTOs
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
