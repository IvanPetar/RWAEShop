using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWAEshopDAL.Models;
using RWAEshopDAL.Services;
using RWAEShopMVC.ViewModels;

namespace RWAEShopMVC.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CategoryController(IMapper mapper, ICategoryService categoryService)
        {
            _mapper = mapper;
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult Index()
        {

            var categories = _categoryService.GetAllCategory();

            var model = _mapper.Map<List<CategoryVM>>(categories);

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(CategoryVM model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            if (_categoryService.GetAllCategory().Any(t => t.Name != null 
            && t.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Name", " A Category with that name already exists");
                return View(model);
            }

            var type = _mapper.Map<ProductCategory>(model);
            _categoryService.CreateCategory(type);

            return RedirectToAction(nameof(Index));
          
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var category = _categoryService.GetCategory(id);
            if (category == null) return NotFound();

            var model = _mapper.Map<CategoryVM>(category);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, CategoryVM model)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }
            if (_categoryService.GetAllCategory().Any(t => t.Name != null
            && t.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Name", " A Category with that name already exists");
                return View(model);
            }

            var updated = _mapper.Map<ProductCategory>(model);
            updated.IdCategory = id;
            _categoryService.UpdateCategory(updated);
            return RedirectToAction(nameof(Index));
            
        }

        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var category = _categoryService.GetCategory(id);
            if (category == null) return NotFound();

            var model = _mapper.Map<CategoryVM>(category);
            return View(model);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var existing = _categoryService.GetCategory(id);
            if (existing != null) _categoryService.DeleteCategory(id);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
