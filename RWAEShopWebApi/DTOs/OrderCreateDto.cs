namespace RWAEShopWebApi.DTOs
{
    public class OrderCreateDto
    {
        public int UserId { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; } = new();
       

    }
    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
