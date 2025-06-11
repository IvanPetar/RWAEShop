using Microsoft.EntityFrameworkCore;
using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly EshopContext _context;

        public ProductRepository(EshopContext context)
        {
            _context = context;
        }

        public void Add(Product product)
        {
            _context.Products.Add(product); 
            _context.SaveChanges();
        }

        public void AddProductToCountry(int productID, int countryID)
        {
            if (!_context.CountryProducts.Any(cp => cp.ProductId == productID && cp.CountryId == countryID)) 
            {
                _context.CountryProducts.Add(new CountryProduct {ProductId = productID , CountryId = countryID});
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var item = _context.Products
                .Include(h => h.OrderItems)
                .Include(h => h.CountryProducts)
                .FirstOrDefault(h => h.IdProduct == id);

            if (item != null) 
            {
                _context.OrderItems.RemoveRange(item.OrderItems);
                _context.CountryProducts.RemoveRange(item.CountryProducts);
                _context.Products.Remove(item);

                _context.SaveChanges();
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products
                .Include(p=> p.CountryProducts)
                .ThenInclude(cp => cp.Country)
                .ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products
                .Include(p=> p.CountryProducts)
                .ThenInclude(cp => cp.Country)
                .FirstOrDefault(p=> p.IdProduct == id);
        }

        public IEnumerable<Country> GetCountriesforProduct(int productID)
        {
            var product = _context.Products
            .Include(p => p.CountryProducts)
                .ThenInclude(cp => cp.Country)
            .FirstOrDefault(p => p.IdProduct == productID);

            return product?.CountryProducts.Select(cp => cp.Country) ?? Enumerable.Empty<Country>();

        }

        public void RemoveProductToCountry(int productID, int countryID)
        {
            var cp = _context.CountryProducts.FirstOrDefault(cp => cp.ProductId == productID && cp.CountryId == countryID);
            if (cp != null)
            {
                _context.CountryProducts.Remove(cp);
                _context.SaveChanges();
            }
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
        public void SaveChanges() => _context.SaveChanges();
    }
}
