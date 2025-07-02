using System.ComponentModel.DataAnnotations;

namespace RWAEShopMVC.ViewModels
{
    public class ProductVM
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity je obavezno polje.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity mora biti veći od 0.")]
        public int Quantity { get; set; }

        public string ImageUrl { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public List<string> CountryNames { get; set; } = new(); 
    }
}
