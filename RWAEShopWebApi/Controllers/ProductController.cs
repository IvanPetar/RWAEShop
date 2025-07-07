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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        public ProductController(IProductService service, IMapper mapper, ILogService logService)
        {
            _service = service;
            _mapper = mapper;
            _logService = logService;

        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductResponseDto>> GetAllProducts([FromQuery] int? categoryId,[FromQuery] string? q,[FromQuery] int page = 1,[FromQuery] int pageSize = 4)
        {
            try
            {
                var product = _service.GetAllProducts(categoryId, page, pageSize);

                if (categoryId.HasValue)
                    product = product.Where(p => p.CategoryId == categoryId.Value);

                var dtos = product.Select(p =>
                {
                    var dto = _mapper.Map<ProductResponseDto>(p);
                    dto.CountryNames = p.CountryProducts
                    .Select(cp => cp.Country.Name)
                    .Where(name => name != null)
                    .ToList();
                    return dto;
                }).ToList();

                _logService.Log($"Retrieved products. Number of retrieved: {dtos.Count}", 1);

                if (!dtos.Any())
                {
                    return NotFound("Did not found any product");
                }

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                _logService.Log($"Failed to retrieve product items: {ex.Message}", 3);
                return StatusCode(500, $"An error occurred while retrieving products: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public ActionResult<ProductResponseDto> GetProductById(int id)
        {
            try
            {
                var product = _service.GetProduct(id);
                if (product == null)
                {
                    _logService.Log($"Attempting to retrieve a non-existent product (ID={id})", 2);
                    return NotFound();
                }

                var dto = _mapper.Map<ProductResponseDto>(product);
                dto.CountryNames = product.CountryProducts.
                    Select(cp => cp.Country.Name).ToList();

                _logService.Log($"Product received: {product.Name} (ID={product.IdProduct})", 1);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logService.Log($"Error retrieving product item ID={id}: {ex.Message}", 3);
                return StatusCode(500, $"An error occurred while retrieving product by id: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<ProductCreateDto> CreateProduct([FromBody] ProductCreateDto dto)
        {
            if (dto == null)
            {
                _logService.Log("Attempting to create a product with empty data!", 2);
                return BadRequest("Invalid data");
            }

            try
            {
                var product = _mapper.Map<Product>(dto);

                product.CountryProducts = dto.CountryId
                    .Select(countryId => new CountryProduct { CountryId = countryId })
                    .ToList();

                _service.CreateProduct(product);

                _logService.Log($"Created new product: {product.Name} (ID={product.IdProduct})", 1);

                return CreatedAtAction(nameof(GetAllProducts), new { id = product.IdProduct }, dto);
            }
            catch (Exception ex)
            {
                _logService.Log($"Error creating product item: {ex.Message}", 3);
                return StatusCode(500, $"An error occurred while creating product: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]

        public ActionResult<ProductUpdateDto> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
        {
            try
            {
                var product = _service.GetProduct(id);
                if (product == null)
                {
                    _logService.Log($"Attempting to update a non-existent product (ID={id})", 2);
                    return NotFound("Did not found product by that Id");
                }

                _mapper.Map(dto, product);
                _service.UpdateProduct(product);

                _logService.Log($"Updated product: {product.Name} (ID={product.IdProduct})", 2);

                var responseDto = _mapper.Map<ProductResponseDto>(product);
                responseDto.CountryNames = product.CountryProducts
                .Select(cp => cp.Country.Name)
                .Where(name => name != null)
                .ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logService.Log($"Error updating product item: {ex.Message}", 3);
                return StatusCode(500, $"An error occurred while updating product: {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var product = _service.GetProduct(id);
                if (product == null)
                {
                    _logService.Log($"Attempting to delete a non-existent product (ID={id})", 2);
                    return NotFound();
                }

                _service.DeleteProduct(id);
                _logService.Log($"Deleted product: {product.Name} (ID={product.IdProduct})", 2);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logService.Log($"Error deleting product item: {ex.Message}", 3);
                return StatusCode(500, $"An error occurred while deleting product: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<ProductResponseDto>> Search([FromQuery] string query)
        {
            try
            {
                var items = _service.GetAllQueryable()
                    .Where(m => EF.Functions.Like(m.Name,$"%{query}%"))
                    .ToList();

                var response = _mapper.Map<IEnumerable<ProductResponseDto>>(items);
                _logService.Log($"Product search by term: '{query}', number of found: {response.Count()}", 1);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logService.Log($"Menu search failed: {ex.Message}", 3);
                return StatusCode(500, ex.Message);
            }
        }
    }
}

