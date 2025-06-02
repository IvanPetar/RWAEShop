using Microsoft.EntityFrameworkCore;
using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EshopContext _context;

        public CategoryRepository(EshopContext context)
        {
            _context = context;
        }

        public void Add(ProductCategory category)
        {
            _context.ProductCategories.Add(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.ProductCategories
                .Include(h => h.Products)
                .FirstOrDefault(h =>h.IdCategory == id);

            if (item != null) 
            {
                _context.Products.RemoveRange(item.Products);
                _context.ProductCategories.Remove(item);

                _context.SaveChanges();
            }

        }

        public IEnumerable<ProductCategory> GetAll()
        {
            return _context.ProductCategories.ToList();
        }

        public ProductCategory? GetById(int id)
        {
            return _context.ProductCategories.Find(id);
        }

        public void Update(ProductCategory category)
        {
            _context.ProductCategories.Update(category);
            _context.SaveChanges();
        }
    }
}
