using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWAEshopDAL.Models;
using RWAEShop.DTOs;

namespace RWAEShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly EshopContext _context;

        public OrderController(EshopContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public ActionResult<IEnumerable<OrderResponseDto>> GetAllOrders()
        {
            try
            {
                var orders = _context.Orders
                    .Include(o => o.OrderItems)
                    .ToList();

                var dtos = orders.Select(order => new OrderResponseDto
                {
                    
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Items = order.OrderItems.Select(item => new OrderItemDto
                    {
                        
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        TotalCost = item.TotalCost
                    }).ToList()
                }).ToList();

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving orders: {ex.Message}");
            }
        }

        
        [HttpGet("{id}")]
        public ActionResult<OrderResponseDto> GetOrderById(int id)
        {
            try
            {
                var order = _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefault(o => o.IdOrder == id);

                if (order == null)
                    return NotFound($"Order with that {id} not found.");

                var dto = new OrderResponseDto
                {
                    
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Items = order.OrderItems.Select(item => new OrderItemDto
                    {  
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        TotalCost = item.TotalCost
                    }).ToList()
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving order by id: {ex.Message}");
            }
        }

       
        [HttpPost]
        public ActionResult<OrderResponseDto> CreateOrder(OrderCreateDto dto)
        {
            try
            {
                var order = new Order
                {
                    UserId = dto.UserId,
                    OrderDate = DateTime.Now,
                    OrderItems = new List<OrderItem>()
                };

                decimal total = 0;

                foreach (var itemDto in dto.Items)
                {
                    var product = _context.Products.Find(itemDto.ProductId);
                    if (product == null)
                        return BadRequest($"Product with that id {itemDto.ProductId} doesnt exist.");

                    decimal price = product.Price;
                    decimal totalCost = (decimal)(price * itemDto.Quantity);

                    var orderItem = new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        Price = price,
                        TotalCost = totalCost
                    };

                    order.OrderItems.Add(orderItem);
                    total += totalCost;
                }

                order.TotalAmount = total;

                _context.Orders.Add(order);
                _context.SaveChanges();

                var resultDto = new OrderResponseDto
                {
                    
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Items = order.OrderItems.Select(item => new OrderItemDto
                    {
                       ProductId= item.ProductId,
                       Quantity = item.Quantity,

                       
                    }).ToList()
                };

                return CreatedAtAction(nameof(GetOrderById), new { id = order.IdOrder }, resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro while creating order: {ex.Message}");
            }
        }

        
        [HttpPut("{id}")]
        public ActionResult<OrderUpdateDto> UpdateOrder(int id, [FromBody] OrderUpdateDto dto)
        {
            try
            {
                var order = _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefault(o => o.IdOrder == id);

                if (order == null)
                    return NotFound($"Order with that  {id} doesnt exist.");

                order.UserId = dto.UserId;

                var existingItems = order.OrderItems.ToDictionary(oi => oi.IdOrderItem);
                var updatedItems = new List<OrderItem>();
                decimal newTotal = 0;

                foreach (var itemDto in dto.Items)
                {
                    var product = _context.Products.Find(itemDto.ProductId);
                    if (product == null)
                        return BadRequest($"Product with that id {itemDto.ProductId} doesnt exist.");

                    var price = product.Price;
                    var totalCost = price * itemDto.Quantity;

                    if (itemDto.IdOrderItem != 0 && existingItems.TryGetValue(itemDto.IdOrderItem, out var existingItem))
                    {
                        existingItem.ProductId = itemDto.ProductId;
                        existingItem.Quantity = itemDto.Quantity;
                        existingItem.Price = price;
                        existingItem.TotalCost = totalCost;
                        updatedItems.Add(existingItem);
                    }
                    else
                    {
                        var newItem = new OrderItem
                        {
                            ProductId = itemDto.ProductId,
                            Quantity = itemDto.Quantity,
                            Price = price,
                            TotalCost = totalCost
                        };
                        updatedItems.Add(newItem);
                    }

                    newTotal += totalCost;
                }

                var updatedIds = dto.Items.Where(i => i.IdOrderItem != 0).Select(i => i.IdOrderItem).ToHashSet();
                var itemsToRemove = order.OrderItems.Where(oi => oi.IdOrderItem != 0 && !updatedIds.Contains(oi.IdOrderItem)).ToList();
                _context.OrderItems.RemoveRange(itemsToRemove);

                order.OrderItems = updatedItems;
                order.TotalAmount = newTotal;

                _context.SaveChanges();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating order: {ex.Message}");
            }
        }

        
        [HttpDelete("{id}")]
        public ActionResult<OrderResponseDto> DeleteOrder(int id)
        {
            try
            {
                var order = _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefault(o => o.IdOrder == id);

                if (order == null)
                    return NotFound($"Order with that {id} didnt found.");

                _context.OrderItems.RemoveRange(order.OrderItems);
                _context.Orders.Remove(order);
                _context.SaveChanges();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting order: {ex.Message}");
            }
        }
    }
}
