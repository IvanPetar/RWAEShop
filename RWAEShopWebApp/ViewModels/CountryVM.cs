using System.ComponentModel.DataAnnotations;

namespace RWAEShopWebApp.ViewModels
{
    public class CountryVM
    {
        public int IdCountry { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Country name")]
        public string Name { get; set; }
    }
}
