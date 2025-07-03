using System.ComponentModel.DataAnnotations;

namespace RWAEShopWebApi.DTOs
{
    public class CategoryResponseDto
    {
        public int IdCategory { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
