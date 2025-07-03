using RWAEshopDAL.Models;
using RWAEshopDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public void AddProductToCountries(int productID, int countryID) => _repository.AddProductToCountry(productID, countryID);

        public void CreateProduct(Product product)=> _repository.Add(product);
        public void DeleteProduct(int id) => _repository.Delete(id);

        public IEnumerable<Product> GetAllProducts(int? categoryId = null, int page = 1, int pageSize = 4)
        {
            return _repository.GetAll(categoryId, page, pageSize);
        }

        public IQueryable<Product> GetAllQueryable()
        {
            return _repository.GetAllQueryable();
        }

        public IEnumerable<Country> GetCountriesforProductes(int productID)=> _repository.GetCountriesforProduct(productID);

        public Product? GetProduct(int id) => _repository.GetById(id);

        public void RemoveProductToCountries(int productID, int countryID)=> _repository.RemoveProductToCountry(productID, countryID);

        public void UpdateProduct(Product product) => _repository.Update(product);
    }
}
