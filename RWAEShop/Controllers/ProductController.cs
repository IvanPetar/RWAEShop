using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWAEShop.DTOs;
using RWAEShop.Models;

namespace RWAEShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly EshopContext _context;

        public ProductController(EshopContext context)
        {
            _context = context;
        }


        [HttpGet]
        public ActionResult<IEnumerable<ProductResponseDto>> GetAllProducts([FromQuery] int? categoryId)
        {
            try
            {
                var product = _context.Products
                    .Include(c => c.Category)
                    .Where(c => !categoryId.HasValue || c.CategoryId == categoryId.Value)
                    .Select(p => new ProductResponseDto
                    {
                        Name = p.Name,
                        Description = p.ProductDescription,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl
                        
                    }).ToList();

                if (!product.Any()) 
                {
                    return NotFound("Did not found any product");
                }

                return Ok(product);
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
                var product = _context.Products
                    .Include(c => c.Category)
                    .FirstOrDefault(c => c.IdProduct == id);
    
                if (product == null) 
                {
                    return NotFound();    
                }

                return Ok(product);
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
                var product = new Product
                {
                    Name = dto.Name,
                    ProductDescription = dto.Description,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    ImageUrl = dto.ImageUrl,
                    CategoryId = dto.CategoryId
                };

                _context.Products.Add(product);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetAllProducts), new {id = product.IdProduct}, product);
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
                var product = _context.Products.FirstOrDefault(c=> c.IdProduct == id);
                if (product == null)
                {
                    return NotFound("Did not found product by that Id");
                }
                product.Name = dto.Name;
                product.ProductDescription = dto.Description;
                product.Price = dto.Price;
                product.Quantity = dto.Quantity;
                product.ImageUrl = dto.ImageUrl;
                product.CategoryId = dto.CategoryId;

                _context.SaveChanges();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating product: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<ProductResponseDto> DeleteProduct(int id) 
        {
            try
            {
                var product = _context.Products.FirstOrDefault(c => c.IdProduct == id);
                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                _context.SaveChanges();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting product: {ex.Message}");
            }
        }
    }
}
