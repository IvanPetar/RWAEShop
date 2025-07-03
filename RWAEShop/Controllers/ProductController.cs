using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWAEShop.DTOs;
using RWAEshopDAL.Models;
using RWAEshopDAL.Services;
using AutoMapper;

namespace RWAEShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public ProductController(IProductService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
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


                if (!dtos.Any())
                {
                    return NotFound("Did not found any product");
                }

                return Ok(dtos);
            }
            catch (Exception ex)
            {

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
                    return NotFound();

                var dto = _mapper.Map<ProductResponseDto>(product);
                dto.CountryNames = product.CountryProducts.
                    Select(cp => cp.Country.Name).ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving product by id: {ex.Message}");
            }
        }


        [HttpPost]
        public ActionResult<ProductCreateDto> CreateProduct([FromBody] ProductCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data");
            }

            try
            {
                var product = _mapper.Map<Product>(dto);

                product.CountryProducts = dto.CountryId
                    .Select(countryId => new CountryProduct { CountryId = countryId })
                    .ToList();

                _service.CreateProduct(product);

              

                return CreatedAtAction(nameof(GetAllProducts), new { id = product.IdProduct }, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating product: {ex.Message}");
            }
        }


        [HttpPut("{id}")]

        public ActionResult<ProductUpdateDto> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
        {
            try
            {
                var product = _service.GetProduct(id);
                if (product == null)
                {
                    return NotFound("Did not found product by that Id");
                }

                _mapper.Map(dto, product);
                _service.UpdateProduct(product);


                var responseDto = _mapper.Map<ProductResponseDto>(product);
                responseDto.CountryNames = product.CountryProducts
                .Select(cp => cp.Country.Name)
                .Where(name => name != null)
                .ToList();

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating product: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                var product = _service.GetProduct(id);
                if (product == null)
                    return NotFound();

                _service.DeleteProduct(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting product: {ex.Message}");
            }
        }

    }
}

