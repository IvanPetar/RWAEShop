using System.ComponentModel.DataAnnotations;

namespace RWAEShop.DTOs
{
    public class OrderUpdateDto
    {
        public int UserId { get; set; }
        public List<UpdateOrderItemDto> OrderItems { get; set; } = new();
    }

    public class UpdateOrderItemDto 
    {
        [Required]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
 

    }
}
