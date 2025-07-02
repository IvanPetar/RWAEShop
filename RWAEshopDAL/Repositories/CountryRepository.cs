using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly EshopContext _context;

        public CountryRepository(EshopContext context)
        {
            _context = context;
        }

        public void Add(Country country)
        {
            _context.Countries.Add(country);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.CountryProducts
                .Where(cp => cp.CountryId == id);

            _context.CountryProducts.RemoveRange(item);

            var country = _context.Countries.Find(id);
            if (country != null)
                _context.Countries.Remove(country);

            _context.SaveChanges();
        }

        public IEnumerable<Country> GetAll()
        {
            return _context.Countries.ToList();
        }

        public Country? GetByID(int id)
        {
            return _context.Countries.Find(id);
        }

        public void Update(Country country)
        {
            var existing = _context.Countries.Find(country.IdCountry);
            if (existing != null) 
            {
                existing.Name = country.Name;
                _context.SaveChanges();
            }
           
        }
        public void SaveChanges() => _context.SaveChanges();
    }
}
