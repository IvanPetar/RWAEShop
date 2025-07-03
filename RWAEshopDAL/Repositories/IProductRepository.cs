using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll(int? categoryId, int page, int pageSize);
        Product? GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        IQueryable<Product> GetAllQueryable();
        IEnumerable<Country> GetCountriesforProduct(int productID);
        void AddProductToCountry(int productID, int countryID);
        void RemoveProductToCountry(int productID, int countryID);
        void SaveChanges();
    }
}
