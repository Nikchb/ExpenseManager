using ServerApp.Models;
using ServerApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.CategoryService
{
    public interface ICategoryService
    {
        public ServiceResponse<IEnumerable<CategoryModel>> Get(string userId);
        public Task<ServiceResponse<CategoryModel>> Get(string userId, string categoryId);
        public Task<ServiceResponse<CategoryModel>> Create(string userId, CreateCategoryModel model);
        public Task<ServiceResponse<CategoryModel>> Update(string userId, CategoryModel model);
        public Task<ServiceResponse<CategoryModel>> Delete(string userId, string categoryId);
    }
}
