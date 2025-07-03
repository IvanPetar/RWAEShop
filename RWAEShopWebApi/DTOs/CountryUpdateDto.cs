using System.ComponentModel.DataAnnotations;

namespace RWAEShopWebApi.DTOs
{
    public class CountryUpdateDto
    {
        [Required]
        public string Name {  get; set; }
    }
}
