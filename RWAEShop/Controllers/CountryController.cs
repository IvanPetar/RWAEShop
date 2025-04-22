using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWAEShop.DTOs;
using RWAEShop.Models;

namespace RWAEShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly EshopContext _context;

        public CountryController(EshopContext context)
        {
            _context = context;
        }

        [HttpGet]

        public ActionResult<IEnumerable<CountryResponseDto>> GetAllCountries()
        {
            var country = _context.Countries
                .Select(c => new CountryResponseDto
                {
                    Id = c.IdCountry,
                    Name = c.Name
                }).ToList();


            return Ok(country);
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<CountryResponseDto>> GetAllCountrieById(int idCountry)
        {
            var country = _context.Countries
                .FirstOrDefault(x => x.IdCountry == idCountry);

            if (country == null) { 
                
                return NotFound();
            }

            return Ok(country);
        }

        [HttpPost]
        public ActionResult<CreateCountryDto> CreateCountry([FromBody] CreateCountryDto dto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Country country = new();
            country.Name = dto.Name;

            _context.Countries.Add(country);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetAllCountrieById), new { id = country.IdCountry}, country);
        }

    } 
}
