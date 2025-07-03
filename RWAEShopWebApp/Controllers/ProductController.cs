using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using RWAEshopDAL.Models;
using RWAEshopDAL.Services;
using RWAEShopWebApp.ViewModels;

namespace RWAEShopWebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;
        private readonly ICountryService _countryService;

        public ProductController(IProductService productService,ICategoryService categoryService, ICountryService countryService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
            _categoryService = categoryService;
            _countryService = countryService;
        }


        public ActionResult Index(string? q, int? categoryId, int page = 1, int pageSize = 4)
        {
            // 1. Dohvati queryable s relacijama
            var query = _productService.GetAllQueryable();

            // 2. Search
            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(p => p.Name != null && EF.Functions.Like(p.Name, $"%{q}%"));

            // 3. Filtriranje po kategoriji
            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            // 4. Ukupno za paginaciju
            var totalCount = query.Count();

            // 5. Paginacija
            var pagedProducts = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // 6. Mapping u ViewModel
            var model = _mapper.Map<List<ProductVM>>(pagedProducts);

            // 7. Popuni kategorije za dropdown
            var categories = _categoryService.GetAllCategory()
                .Select(c => new SelectListItem
                {
                    Value = c.IdCategory.ToString(),
                    Text = c.Name
                }).ToList();
            ViewBag.CategoryList = new SelectList(categories, "Value", "Text");

            // 8. Podaci za paginaciju/filter
            ViewData["CurrentFilter"] = q;
            ViewData["CurrentCategory"] = categoryId;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;

            return View(model);
        }


        public ActionResult Details(int id)
        {
            var item = _productService.GetProduct(id);
            if (item == null) 
            {
                return NotFound();
            }
            var model = _mapper.Map<ProductVM>(item);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_categoryService.GetAllCategory(), "IdCategory", "Name");
            ViewBag.Countries = new MultiSelectList(_countryService.GetAllCountry(), "IdCountry", "Name");

            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductVM model)
        {
            
                if (!ModelState.IsValid) 
                {
                    ViewBag.CategoryId = new SelectList(_categoryService.GetAllCategory(), "IdCategory", "Name", model.CategoryId);
                    ViewBag.Countries = new MultiSelectList(_countryService.GetAllCountry(), "IdCountry", "Name", model.CountryNames);
                    return View(model);
                }

            if(model.ImageFile != null && model.ImageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await model.ImageFile.CopyToAsync(ms);
                var imageBytes = ms.ToArray();
                model.ImageUrl = $"data:{model.ImageFile.ContentType};base64,{Convert.ToBase64String(imageBytes)}";
            }


                var product = _mapper.Map<Product>(model);

                _productService.CreateProduct(product);

            foreach (var countryStr in model.CountryNames)
            {
                if (int.TryParse(countryStr, out int countryId))
                {
                    _productService.AddProductToCountries(product.IdProduct, countryId);
                }
            }

            return RedirectToAction(nameof(Index));

        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var product = _productService.GetProduct(id);
            if (product == null)
                return NotFound();

            var model = _mapper.Map<ProductVM>(product);

            model.CountryNames = product.CountryProducts?
                .Select(cp => cp.Country.Name)
                .ToList() ?? new List<string>();

            ViewBag.CategoryId = new SelectList(_categoryService.GetAllCategory(), "IdCategory", "Name", model.CategoryId);
            ViewBag.Countries = new MultiSelectList(_countryService.GetAllCountry(), "IdCountry", "Name", model.CountryNames);

            return View(model);
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, ProductVM model)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(_categoryService.GetAllCategory(), "IdCategory", "Name", model.CategoryId);
                ViewBag.Countries = new MultiSelectList(_countryService.GetAllCountry(), "IdCountry", "Name", model.CountryNames);
                return View(model);
            }


            var existingProduct = _productService.GetProduct(id);
            if (existingProduct == null)
                return NotFound();

            _mapper.Map(model, existingProduct);

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                using var ms = new MemoryStream();
                await model.ImageFile.CopyToAsync(ms);
                var imageBytes = ms.ToArray();
                existingProduct.ImageUrl = $"data:{model.ImageFile.ContentType};base64,{Convert.ToBase64String(imageBytes)}";
            }

            

            foreach (var countryName in model.CountryNames)
            {
                var country = _countryService.GetAllCountry().FirstOrDefault(c => c.Name == countryName);
                if (country != null)
                {
                    existingProduct.CountryProducts.Add(new CountryProduct
                    {
                        CountryId = country.IdCountry,
                        ProductId = existingProduct.IdProduct
                    });
                }
            }

            _productService.UpdateProduct(existingProduct);

            return RedirectToAction(nameof(Index));


        }

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var item = _productService.GetProduct(id);
            if (item == null) return NotFound();

            var model = _mapper.Map<ProductVM>(item);

            model.CountryNames = item.CountryProducts?
                .Select(cp => cp.Country.Name)
                .ToList() ?? new List<string>();

            // Prikaži potvrdu za brisanje
            return View(model);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                _productService.DeleteProduct(id);
               
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var item = _productService.GetProduct(id);
                var model = _mapper.Map<ProductVM>(item);
                ModelState.AddModelError("", "An error occurred while deleting the product.");
                return View(model);
            }
        }

       
        [HttpGet]
        public IActionResult Shop(string? q, int? categoryId, int page = 1, int pageSize = 4)
        {
            var query = _productService.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(p => p.Name != null && EF.Functions.Like(p.Name, $"%{q}%"));

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            var totalCount = query.Count();
            var pagedProducts = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var model = _mapper.Map<List<ProductVM>>(pagedProducts);

            var categories = _categoryService.GetAllCategory()
                .Select(c => new SelectListItem
                {
                    Value = c.IdCategory.ToString(),
                    Text = c.Name
                }).ToList();
            ViewBag.CategoryList = new SelectList(categories, "Value", "Text");

            ViewData["CurrentFilter"] = q;
            ViewData["CurrentCategory"] = categoryId;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalCount / pageSize);
            ViewData["Page"] = page;
            ViewData["PageSize"] = pageSize;

            return View(model);
        }

        [HttpGet]
        public IActionResult ShopDetails(int id)
        {
            var product = _productService.GetProduct(id);
            if (product == null)
                return NotFound();

            var model = _mapper.Map<ProductVM>(product);
            return View(model);
        }
    }
}
