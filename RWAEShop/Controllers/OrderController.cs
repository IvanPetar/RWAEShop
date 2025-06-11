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
                return StatusCode(500, $"Erro while creating order: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public ActionResult<OrderResponseDto> UpdateOrder(int id, [FromBody] OrderUpdateDto request)
        {
            try
            {
                
                var order = _service.UpdateOrder(id, productId, quantity);
                if (order == null) return NotFound("Didnt found");

                var response = _mapper.Map<OrderResponseDto>(order);    

                return NoContent();
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
