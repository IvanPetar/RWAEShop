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
            try
            {
                var country = _context.Countries
                        .Select(c => new CountryResponseDto
                        {
                            Id = c.IdCountry,
                            Name = c.Name
                        }).ToList();


                return Ok(country);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while retrieving country: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<CountryResponseDto>> GetAllCountrieById(int id)
        {
            try
            {
                var country = _context.Countries
                        .FirstOrDefault(x => x.IdCountry == id);

                if (country == null)
                {

                    return NotFound("Did not found any country by that id");
                }

                return Ok(country);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while retrieving country by id: {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<CreateCountryDto> CreateCountry([FromBody] CreateCountryDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data");
            }

            try
            {

                var country = new Country
                {
                    Name = dto.Name
                };

                _context.Countries.Add(country);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetAllCountrieById), new { id = country.IdCountry}, country);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while creating country: {ex.Message}");
            }

        }



        [HttpPut("{id}")]

        public ActionResult<CountryUpdateDto> UpdateCountry(int id, [FromBody] CountryUpdateDto dto) 
        {
            try
            {
                var country = _context.Countries.FirstOrDefault(c => c.IdCountry == id);
                if (country == null) 
                {
                    return NotFound("Did not found any country");
                }

                country.Name = dto.Name;
                _context.SaveChanges();
                return Ok(country);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while updating country: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]

        public ActionResult<CountryResponseDto> DeleteCountry(int id) 
        {
            try
            {
                var country = _context.Countries.FirstOrDefault(c => c.IdCountry == id);
                if (country == null)
                {
                    return NotFound("Did not found any country");
                }

                var hasProduct = _context.Products.Any(p=>p.IdProduct == id);
                if (hasProduct) 
                {
                    return BadRequest("Cannot delete any Country which Products came form that country");
                }

                _context.Countries.Remove(country);
                _context.SaveChanges();
                return Ok(country);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while deleting country: {ex.Message}");
            }
        }

    } 
}
