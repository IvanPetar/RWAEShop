using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RWAEshopDAL.Models;
using RWAEshopDAL.Services;
using RWAEShopWebApp.ViewModels;

namespace RWAEShopWebApp.Controllers
{
    [Authorize]
    public class CountryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICountryService _countryService;
        private readonly IProductService _productService;

        public CountryController(IMapper mapper, ICountryService countryService, IProductService productService)
        {
            _mapper = mapper;
            _countryService = countryService;
            _productService = productService;
        }



        public ActionResult Index()
        {
            var countries = _countryService.GetAllCountry();
            var model = _mapper.Map<List<CountryVM>>(countries);
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
        public ActionResult Create(CountryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_countryService.GetAllCountry().Any(t => t.Name != null
            && t.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Name", " A Country with that name already exists");
                return View(model);
            }

            var type = _mapper.Map<Country>(model);
            _countryService.CreateCountry(type);

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            var country = _countryService.GetCountry(id);
            if (country == null) return NotFound();

            var model = _mapper.Map<CountryVM>(country);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, CountryVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (_countryService.GetAllCountry().Any(t => t.Name != null
            && t.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("Name", " A Country with that name already exists");
                return View(model);
            }

            var updated = _mapper.Map<Country>(model);
            updated.IdCountry = id;
            _countryService.UpdateCountry(updated);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            var country = _countryService.GetCountry(id);
            if (country == null) return NotFound();

            var model = _mapper.Map<CountryVM>(country);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            var existing = _countryService.GetCountry(id);
            if (existing != null)
                _countryService.DeleteCountry(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
