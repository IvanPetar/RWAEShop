using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWAEshopDAL.Models;
using RWAEShop.DTOs;
using RWAEshopDAL.Services;
using AutoMapper;

namespace RWAEShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public OrderController(IOrderService service, IProductService productService, IMapper mapper)
        {
            _service = service;
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<OrderResponseDto>> GetAllOrders()
        {
            try
            {

                var orders = _service.GetAllOrders();
                var dtos =  _mapper.Map<List<OrderResponseDto>>(orders);

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
                var order = _service.GetOrder(id);
                if (order == null)
                    return NotFound($"Order with that {id} not found.");

                var dto = _mapper.Map<OrderResponseDto>(order);

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
                var order = _mapper.Map<Order>(dto);
                order.OrderDate = DateTime.Now;

                decimal total = 0;

                foreach (var itemDto in order.OrderItems)
                {
                    var product = _productService.GetProduct(itemDto.ProductId);
                    if (product == null)
                        return BadRequest($"Product with that id {itemDto.ProductId} doesnt exist.");

                    itemDto.Price = product.Price;
                    total = itemDto.Price * itemDto.Quantity;

                }

                order.TotalAmount = total;

                _service.CreateOrder(order);
               
                var resultDto = _mapper.Map<OrderResponseDto>(order);

                return CreatedAtAction(nameof(GetOrderById), new { id = order.IdOrder }, resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro while creating order: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public ActionResult<OrderResponseDto> UpdateOrder(int id, [FromBody] OrderUpdateDto dto)
        {
            try
            {
                var order = _service.GetOrder(id);
                if (order == null)
                    return NotFound($"Order with that  {id} doesnt exist.");

                order.UserId = dto.UserId;

                var existingItems = order.OrderItems.ToDictionary(oi => oi.IdOrderItem);
                var updatedItems = new List<OrderItem>();
                decimal newTotal = 0;

                foreach (var itemDto in dto.OrderItems)
                {
                    var product = _productService.GetProduct(itemDto.ProductId);
                    if (product == null)
                        return BadRequest($"Product with that id {itemDto.ProductId} doesnt exist.");

                    var price = product.Price;
                    var totalCost = price * itemDto.Quantity;

                    var newItem = new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity
                    };

                    updatedItems.Add(newItem);
                    newTotal += totalCost;

                }

                order.OrderItems = updatedItems;
                order.TotalAmount = newTotal;

                _service.UpdateOrder(order);

                var responseDto = _mapper.Map<OrderResponseDto>(order);
                return Ok(responseDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while updating order: {ex.Message}");
            }
        }


        //[HttpDelete("{id}")]
        //public ActionResult<OrderResponseDto> DeleteOrder(int id)
        //{
        //    try
        //    {
        //        var order = _context.Orders
        //            .Include(o => o.OrderItems)
        //            .FirstOrDefault(o => o.IdOrder == id);

        //        if (order == null)
        //            return NotFound($"Order with that {id} didnt found.");

        //        _context.OrderItems.RemoveRange(order.OrderItems);
        //        _context.Orders.Remove(order);
        //        _context.SaveChanges();

        //        return Ok(order);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error while deleting order: {ex.Message}");
        //    }
        //}
    }
}
