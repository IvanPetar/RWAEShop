using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<ProductCategory> GetAll();
        ProductCategory? GetById(int id);
        void Add(ProductCategory category);
        void Update(ProductCategory category);
        void Delete(int id);

    }
}
