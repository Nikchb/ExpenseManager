using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using ServerApp.Data;
using ServerApp.Data.Models;
using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResponse> Get(string userId)
        {
            return new SucceededServiceResponse(context.Categories.Where(v => v.UserId == userId).ToArray());
        }

        public async Task<ServiceResponse> Get(string userId, string categoryId)
        {
            var category = await context.Categories.FindAsync(categoryId);
            if(category == null)
            {
                return new NotSucceededServiceResponse(new { Message = "Category not Found" });
            }
            if(category.UserId != userId)
            {
                return new NotSucceededServiceResponse(new { Message = "Access Forbidden" });
            }
            return new SucceededServiceResponse(mapper.Map<Category, CategoryModel>(category));
        }

        public async Task<ServiceResponse> Create(string userId, CreateCategoryModel model)
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
                return new NotSucceededServiceResponse(new { Message = "Creation Failed" });
            }
            return new SucceededServiceResponse(mapper.Map<Category, CategoryModel>(category));
        }

        public async Task<ServiceResponse> Update(string userId, CategoryModel model)
        {            
            var category = await context.Categories.FindAsync(model.Id);
            if (category == null)
            {
                return new NotSucceededServiceResponse(new { Message = "Category not Found" });
            }
            if (category.UserId != userId)
            {
                return new NotSucceededServiceResponse(new { Message = "Access Forbidden" });
            }
            mapper.Map(model, category);
            try
            {                
                await context.SaveChangesAsync();
            }
            catch
            {
                return new NotSucceededServiceResponse(new { Message = "Update Failed" });
            }
            return new SucceededServiceResponse(mapper.Map<Category, CategoryModel>(category));
        }

        public async Task<ServiceResponse> Delete(string userId, string categoryId)
        {
            var category = await context.Categories.FindAsync(categoryId);
            if (category == null)
            {
                return new NotSucceededServiceResponse(new { Message = "Category not Found" });
            }
            if (category.UserId != userId)
            {
                return new NotSucceededServiceResponse(new { Message = "Access Forbidden" });
            }
            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
            }
            catch
            {
                return new NotSucceededServiceResponse(new { Message = "Delete Failed" });
            }
            return new SucceededServiceResponse();
        }         
    }
}
