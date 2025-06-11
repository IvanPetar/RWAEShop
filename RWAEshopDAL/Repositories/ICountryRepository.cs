using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Repositories
{
    public interface ICountryRepository
    {
        IEnumerable<Country> GetAll();
        Country? GetByID(int id);
        void Add(Country country);
        void Update(Country country);
        void Delete(int id);
        void SaveChanges();
    }
}
