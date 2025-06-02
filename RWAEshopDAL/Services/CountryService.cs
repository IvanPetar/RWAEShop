using RWAEshopDAL.Models;
using RWAEshopDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public void CreateCountry(Country country)=> _countryRepository.Add(country);

        public void DeleteCountry(int id)=> _countryRepository.Delete(id);

        public IEnumerable<Country> GetAllCountry()=> _countryRepository.GetAll();

        public Country? GetCountry(int id)=> _countryRepository.GetByID(id);
        public void UpdateCountry(Country country)=> _countryRepository.Update(country);
    }
}
