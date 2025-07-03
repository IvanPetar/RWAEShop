using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Services
{
    public interface IProductService
    {
        IEnumerable<Product> GetAllProducts(int? categoryId, int page, int pageSize);
        Product? GetProduct(int id);
        void CreateProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(int id);
        IQueryable<Product> GetAllQueryable();

        IEnumerable<Country> GetCountriesforProductes(int productID);
        void AddProductToCountries(int productID, int countryID);
        void RemoveProductToCountries(int productID, int countryID);
    }
}
