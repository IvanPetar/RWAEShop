namespace RWAEShop.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? TotalCost { get; set; }

    }
}
