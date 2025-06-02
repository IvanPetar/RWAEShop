using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Services
{
    public interface ICategoryService
    {
        IEnumerable<ProductCategory> GetAllCategory();
        ProductCategory? GetCategory(int id);
        void CreateCategory(ProductCategory category);
        void UpdateCategory(ProductCategory category);
        void DeleteCategory(int id);
    }
}
