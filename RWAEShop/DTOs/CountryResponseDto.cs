using System.ComponentModel.DataAnnotations;

namespace RWAEShop.DTOs
{
    public class CountryResponseDto
    {
        public int IdCountry { get; set; }
        [Required]
        public string Name { get; set;}

    }
}
