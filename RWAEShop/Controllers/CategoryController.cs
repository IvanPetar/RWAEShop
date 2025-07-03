using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RWAEShop.DTOs;
using RWAEshopDAL.Models;
using RWAEshopDAL.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace RWAEShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _service;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IProductService service, IMapper mapper)
        {
            _categoryService = categoryService;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoryResponseDto>> GetAllCategories()
        {
            try
            {
                var categories = _categoryService.GetAllCategory()
                    .Select(p =>
                    _mapper.Map<CategoryResponseDto>(p)
                    ).ToList();

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
                var category = _categoryService.GetCategory(id);
                if (category == null)
                {
                    return NotFound();
                }

                var mappedCategory = _mapper.Map<CategoryResponseDto>(category);
                return Ok(mappedCategory);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving category by id: {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateCategory([FromBody] CategoryCreateDto dto)
        {
            var category = _mapper.Map<ProductCategory>(dto);
            if (dto == null)
            {
                return BadRequest("Invalid category data.");
            }

            try
            {

                _categoryService.CreateCategory(category);

                return CreatedAtAction(nameof(GetCategoriesById), new { id = category.IdCategory}, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating category: {ex.Message}");

            }

        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult<CategoryUpdateDto> UpdateCategory(int id, [FromBody] CategoryUpdateDto dto)
        {
            try
            {
                var category = _categoryService.GetCategory(id);
                if (category == null)
                {
                    return NotFound();
                }

                _mapper.Map(dto, category);

                _categoryService.UpdateCategory(category);

                var updateDto = _mapper.Map<CategoryUpdateDto>(category);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating category: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult<CategoryUpdateDto> DeleteCategory(int id)
        {
            try
            {
                var category = _categoryService.GetCategory(id);
                if (category == null)
                {
                    return NotFound();
                }



                var hasItems = _service.GetAllProducts(id,1, int.MaxValue);
                if (hasItems.Any())
                {
                    return BadRequest("Cannot delete category with existing products. ");
                }

                _categoryService.DeleteCategory(id);
                var dto = _mapper.Map<CategoryUpdateDto>(category); 

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting product: {ex.Message}");
            }
        }
    }
}

