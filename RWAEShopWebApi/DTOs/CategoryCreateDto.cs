using System.ComponentModel.DataAnnotations;

namespace RWAEShopWebApi.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
