using System.ComponentModel.DataAnnotations;

namespace RWAEShop.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
