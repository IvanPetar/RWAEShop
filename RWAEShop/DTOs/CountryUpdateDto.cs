using System.ComponentModel.DataAnnotations;

namespace RWAEShop.DTOs
{
    public class CountryUpdateDto
    {
        [Required]
        public string Name {  get; set; }
    }
}
