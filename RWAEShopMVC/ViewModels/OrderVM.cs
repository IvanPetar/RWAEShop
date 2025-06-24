namespace RWAEShopMVC.ViewModels
{
    public class OrderVM
    {
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemVM> OrderItems { get; set; } = new();
    }
}
