using RWAEshopDAL.Models;
using RWAEshopDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void CreateCategory(ProductCategory category)=> _categoryRepository.Add(category);

        public void DeleteCategory(int id)=> _categoryRepository.Delete(id);  

        public IEnumerable<ProductCategory> GetAllCategory()=> _categoryRepository.GetAll();    

        public ProductCategory? GetCategory(int id)=> _categoryRepository.GetById(id);
        public void UpdateCategory(ProductCategory category)=> _categoryRepository.Update(category);
    }
}
