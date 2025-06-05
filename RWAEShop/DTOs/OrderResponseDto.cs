namespace RWAEShop.DTOs
{
    public class OrderResponseDto
    {      
        public int UserId { get; set; }           
        public DateTime OrderDate { get; set; }   
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
