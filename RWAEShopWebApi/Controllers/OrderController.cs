using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWAEshopDAL.Models;
using RWAEshopDAL.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using RWAEShopWebApi.DTOs;

namespace RWAEShopWebApi.Controllers
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
        [Authorize]
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

        [Authorize]
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
                    if (product.Quantity < itemDto.Quantity)
                    {
                        return BadRequest($"There is not enough product {product.Name} on warehouse.");
                    }

                    product.Quantity -= itemDto.Quantity;
                    _productService.UpdateProduct(product);

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
                return StatusCode(500, $"Error while creating order: {ex.Message}");
            }
        }
        [Authorize]
        [HttpPut("{orderId}/items/{productId}")]
        public ActionResult<OrderResponseDto> UpdateOrderItem(int orderId, int productId, [FromBody] UpdateOrderItemDto dto)
        {
            try
            {
                if (dto.ProductId != productId)
                {
                    return BadRequest("Product ID in URL and body do not match.");
                }

                var order = _service.GetOrder(orderId);
                if (order == null)
                    return NotFound($"Order {orderId} not found.");

                var oldItem = order.OrderItems.FirstOrDefault(oi => oi.ProductId == productId);
                if (oldItem == null)
                    return NotFound($"Product {productId} not found in order.");

                var product = _productService.GetProduct(productId);
                if (product == null)
                    return BadRequest($"Product {productId} not found.");

               
                product.Quantity += oldItem.Quantity;

                if (product.Quantity < dto.Quantity)
                    return BadRequest("Not enough quantity available.");

               
                product.Quantity -= dto.Quantity;
                _productService.UpdateProduct(product);

                oldItem.Quantity = dto.Quantity;
                oldItem.Price = product.Price;

                
                order.TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.Price);
                _service.UpdateOrder(order);

                var resultDto = _mapper.Map<OrderResponseDto>(order);
                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating order item: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult<OrderResponseDto> DeleteOrder(int id)
        {
            try
            {
                var order = _service.GetOrder(id);
                if (order == null)
                    return NotFound($"Order with that {id} didnt found.");
                    
                foreach (var itemDto in order.OrderItems)
                {
                    var product = _productService.GetProduct(itemDto.ProductId);
                    if(product != null)
                    {
                        product.Quantity += itemDto.Quantity;
                        _productService.UpdateProduct(product);
                    }
                }

                _service.DeleteOrder(id);

                return Ok($"Order succesfully deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error while deleting order: {ex.Message}");
            }
        }
    }
}
