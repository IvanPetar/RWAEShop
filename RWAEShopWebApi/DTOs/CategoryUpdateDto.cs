using System.ComponentModel.DataAnnotations;

namespace RWAEShopWebApi.DTOs
{
    public class CategoryUpdateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
