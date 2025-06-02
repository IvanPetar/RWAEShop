namespace RWAEShop.DTOs
{
    public class OrderUpdateDto
    {
        public int UserId { get; set; }
        public List<UpdateOrderItemDto> Items { get; set; }
    }

    public class UpdateOrderItemDto 
    {
        public int IdOrderItem {  get; set; }
        public int OrderId {  get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }


    }
}
