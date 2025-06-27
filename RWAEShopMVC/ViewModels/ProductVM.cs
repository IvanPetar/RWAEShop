namespace RWAEShopMVC.ViewModels
{
    public class ProductVM
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public List<string> CountryNames { get; set; } = new(); 
    }
}
