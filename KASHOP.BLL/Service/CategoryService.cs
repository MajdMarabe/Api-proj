using KASHOP.DAL.dto.request;
using KASHOP.DAL.dto.response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _CategoryRepository;
        public CategoryService(ICategoryRepository CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;

        }
        public async Task< List<CategoryResponse>> GetAll()
        {
            var categories=await _CategoryRepository.GetAllAsync(new string[] {nameof(Category.Translations)});
            return categories.Adapt<List<CategoryResponse>>();
        }
        public async Task<CategoryResponse> Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
           await _CategoryRepository.CreateAsync(category);
            return category.Adapt<CategoryResponse>();
        }
        public async Task<CategoryResponse ?> GetCategory(Expression<Func<Category,bool>> filter )
        {
          var category= await _CategoryRepository.GetOne(new string[] { nameof(Category.Translations) },filter);
            return category.Adapt<CategoryResponse>();
        }

    }
}
