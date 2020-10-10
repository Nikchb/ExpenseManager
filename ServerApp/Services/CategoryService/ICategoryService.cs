using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<ServiceResponse> Get(string userId);
        public Task<ServiceResponse> Get(string userId, string categoryId);
        public Task<ServiceResponse> Create(string userId, CreateCategoryModel model);
        public Task<ServiceResponse> Update(string userId, CategoryModel model);
        public Task<ServiceResponse> Delete(string userId, string categoryId);
    }
}
