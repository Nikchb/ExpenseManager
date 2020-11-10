using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ServerApp.Data;
using ServerApp.Data.Models;
using ServerApp.Models;
using ServerApp.Models.CategoryModels;
using ServerApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.CategoryService
{
    public class CategoryService : ServiceBase<CategoryModel>, ICategoryService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public ServiceResponse<IEnumerable<CategoryModel>> Get(string userId)
        {
            return Success(context.Categories
                .Where(v => v.UserId == userId)
                .Select(v => mapper.Map<Category, CategoryModel>(v))
                .ToArray());
        }

        public async Task<ServiceResponse<CategoryModel>> Get(string userId, string categoryId)
        {
            var category = await context.Categories.FindAsync(categoryId);
            if(category == null)
            {
                return Error("Category not Found");
            }
            if(category.UserId != userId)
            {
                return Error("Access Forbidden");
            }
            return Success(mapper.Map<Category, CategoryModel>(category));
        }

        public async Task<ServiceResponse<CategoryModel>> Create(string userId, CreateCategoryModel model)
        {
            var category = mapper.Map<CreateCategoryModel, Category>(model);
            category.UserId = userId;
            try
            {
                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();
            }
            catch
            {
                return Error("Creation Failed");
            }
            return Success(mapper.Map<Category, CategoryModel>(category));
        }

        public async Task<ServiceResponse<CategoryModel>> Update(string userId, CategoryModel model)
        {            
            var category = await context.Categories.FindAsync(model.Id);
            if (category == null)
            {
                return Error("Category not Found");
            }
            if (category.UserId != userId)
            {
                return Error("Access Forbidden");
            }
            mapper.Map(model, category);
            try
            {                
                await context.SaveChangesAsync();
            }
            catch
            {
                return Error("Update Failed"); 
            }
            return Success(mapper.Map<Category, CategoryModel>(category));
        }

        public async Task<ServiceResponse<CategoryModel>> Delete(string userId, string categoryId)
        {
            var category = await context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return Error("Category not Found");
            }
            if (category.UserId != userId)
            {
                return Error("Access Forbidden");
            }
            if((await context.Records.FirstOrDefaultAsync(v=>v.CategoryId == category.Id)) != null)
            {
                return Error("Delete Rejected");
            }
            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
            catch
            {
                return Error("Delete Failed");
            }
            return Success();
        }         
    }
}
