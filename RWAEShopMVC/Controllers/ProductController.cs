using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RWAEshopDAL.Services;
using RWAEShopMVC.ViewModels;

namespace RWAEShopMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;
        private readonly CategoryService _categoryService;

        public ProductController(ProductService productService,CategoryService categoryService ,IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
            _categoryService = categoryService;
        }


        // GET: ProductController
        public ActionResult Index()
        {

            var products = 
                _productService.GetAllProducts();

            var model = _mapper.Map<List<ProductVM>>(products);
            return View(model);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
