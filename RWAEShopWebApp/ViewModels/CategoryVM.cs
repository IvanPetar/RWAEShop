using System.ComponentModel.DataAnnotations;

namespace RWAEShopWebApp.ViewModels
{
    public class CategoryVM
    {
        public int IdCategory { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name="Category name")]
        public string Name { get; set; }
    }
}
