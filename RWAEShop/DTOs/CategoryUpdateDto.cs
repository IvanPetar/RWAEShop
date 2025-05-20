using System.ComponentModel.DataAnnotations;

namespace RWAEShop.DTOs
{
    public class CategoryUpdateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
