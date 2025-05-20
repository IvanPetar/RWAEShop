using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RWAEShop.DTOs;
using RWAEShop.Models;

namespace RWAEShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly EshopContext _context;

        public CategoryController(EshopContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public ActionResult<IEnumerable<CategoryResponseDto>> GetAllCategories()
        {
            try
            {
                var categories = _context.ProductCategories
                    .Select(p => new CategoryResponseDto 
                    {
                        Id = p.IdCategory,
                        Name = p.Name
                    }).ToList();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving categories: {ex.Message}");
            }
        }

       
        [HttpGet("{id}")]
        public ActionResult<CategoryResponseDto> GetCategoriesById(int id)
        {
            try
            {
                var category = _context.ProductCategories.FirstOrDefault(c => c.IdCategory == id);
                if (category == null)
                {
                    return NotFound();
                }

                return Ok(category);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving category by id: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult CreateCategory([FromBody] CategoryCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid category data.");
            }

            try
            {

                var category = new ProductCategory
                {
                    Name = dto.Name
                };

                _context.ProductCategories.Add(category);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetCategoriesById), new { id = category.IdCategory}, category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating category: {ex.Message}");

            }

        }

        [HttpPut("{id}")]
        public ActionResult<CategoryUpdateDto> UpdateCategory(int id, [FromBody] CategoryUpdateDto dto)
        {
            try
            {
                var category = _context.ProductCategories.FirstOrDefault(c => c.IdCategory == id);
                if (category == null)
                {
                    return NotFound();
                }
                category.Name = dto.Name;
                _context.SaveChanges();
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating category: {ex.Message}");
            }
        }

       
        [HttpDelete("{id}")]
        public ActionResult<CategoryUpdateDto> DeleteCategory(int id)
        {
            try
            {
                var category = _context.ProductCategories.FirstOrDefault(c => c.IdCategory == id);
                if (category == null)
                {
                    return NotFound();
                }

                var hasItems = _context.Products.Any(p => p.IdProduct == id);
                if (hasItems)
                {
                    return BadRequest("Cannot delete category with existing products. ");
                }

                _context.ProductCategories.Remove(category);
                _context.SaveChanges();

                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting product: {ex.Message}");
            }
        }
    }
}

