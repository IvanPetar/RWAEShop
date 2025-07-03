using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RWAEshopDAL.Models;
using RWAEshopDAL.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using RWAEShopWebApi.DTOs;

namespace RWAEShopWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
       private readonly ICountryService _countryService;
       private readonly IProductService _service;
       private readonly IMapper _mapper;

        public CountryController(ICountryService countryService, IProductService service,IMapper mapper)
        {
            _countryService = countryService;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]

        public ActionResult<IEnumerable<CountryResponseDto>> GetAllCountries()
        {
            try
            {
                var country = _countryService.GetAllCountry()
                        .Select(c => _mapper.Map<CountryResponseDto>(c)).ToList();


                return Ok(country);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while retrieving country: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<CountryResponseDto>> GetAllCountriesById(int id)
        {
            try
            {
                var country = _countryService.GetCountry(id);

                if (country == null)
                {

                    return NotFound("Did not found any country by that id");
                }
                var mappedCountry = _mapper.Map<CountryResponseDto>(country);
                return Ok(country);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while retrieving country by id: {ex.Message}");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult<CreateCountryDto> CreateCountry([FromBody] CreateCountryDto dto)
        {
           
            var country = _mapper.Map<Country>(dto);
            if (dto == null)
            {
                return BadRequest("Invalid data");
            }

            try
            {

                _countryService.CreateCountry(country);

                return CreatedAtAction(nameof(GetAllCountriesById), new { id = country.IdCountry}, country);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while creating country: {ex.Message}");
            }

        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]

        public ActionResult<CountryUpdateDto> UpdateCountry(int id, [FromBody] CountryUpdateDto dto) 
        {
           
            try
            {
                var country = _countryService.GetCountry(id);
                if (country == null) 
                {
                    return NotFound("Did not found any country");
                }

                _mapper.Map(dto, country);
                _countryService.UpdateCountry(country);
                var updateDto = _mapper.Map<CountryUpdateDto>(country);
                return Ok(updateDto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while updating country: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]

        public ActionResult<CountryResponseDto> DeleteCountry(int id) 
        {
            try
            {
                var country = _countryService.GetCountry(id);
                if (country == null)
                {
                    return NotFound("Did not found any country");
                }

                var products = _service.GetCountriesforProductes(id);
                if (products.Any()) 
                {
                    return BadRequest("Cannot delete any Country which Products came form that country");
                }

                _countryService.DeleteCountry(id);
                var dto = _mapper.Map<CountryResponseDto>(country);
                return Ok(dto);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An error occurred while deleting country: {ex.Message}");
            }
        }

    } 
}
