using System.ComponentModel.DataAnnotations;

namespace RWAEShopWebApi.DTOs
{
    public class CreateCountryDto
    {
        [StringLength(70)]
        [Required(ErrorMessage = "Country is required")]

        public string Name { get; set; } = null!;
    }
}
