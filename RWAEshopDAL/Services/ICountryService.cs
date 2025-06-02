using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Services
{
    public interface ICountryService
    {
        IEnumerable<Country> GetAllCountry();
        Country? GetCountry(int id);
        void CreateCountry(Country country);
        void UpdateCountry(Country country);
        void DeleteCountry(int id);
    }
}
