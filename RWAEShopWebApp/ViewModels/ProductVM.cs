using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RWAEShopWebApp.ViewModels
{
    public class ProductVM
    {
        public int IdProduct { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Product name")]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "Product description")]
        public string ProductDescription { get; set; }
        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity required!")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
        public int Quantity { get; set; }

        [Display(Name = "Upload Image")]
        [ValidateNever]
        public IFormFile? ImageFile { get; set; }

        [ValidateNever]
        public string? ImageUrl { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
       
        public string? CategoryName { get; set; }

        [Required]
        [Display(Name = "Countries")]
        public List<string> CountryNames { get; set; } = new(); 

    }
}
