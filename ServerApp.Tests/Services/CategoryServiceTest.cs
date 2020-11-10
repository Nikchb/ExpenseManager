using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ServerApp.Data;
using ServerApp.Data.Models;
using ServerApp.Mapper;
using ServerApp.Models.CategoryModels;
using ServerApp.Services.AuthService;
using ServerApp.Services.CategoryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Tests.Services
{
    [TestFixture]
    public class CategoryServiceTest : BaseTest
    {
        private readonly string userId = "1";

        private readonly string wrongUserId = "2";

        private readonly string categoryId = "1";
        private readonly decimal monthlyLimit = 1000;
        private readonly string title = "Get";

        private readonly string wrongCategoryId = "2";

        private ICategoryService categoryService;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            context.Users.Add(new User
            {
                Id = userId,
                UserName = "Name",
                PasswordHash = "Password",
                Name = "Name",
                Bill = 0,
                Lang = "ru-RU"
            });
            context.Categories.Add(new Category
            {
                Id = categoryId,
                UserId = userId,
                MonthlyLimit = monthlyLimit,
                Title = title
            });            
            context.SaveChanges();
            categoryService = new CategoryService(context, mapper);
        }

        [Test]
        public void GetAll()
        {
            var result = categoryService.Get(userId);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(1, result.Response.Count());
            Assert.IsTrue(
                result.Response.Any(v=>
                    v.Id == categoryId &&
                    v.MonthlyLimit == monthlyLimit && 
                    v.Title == title
                )
           );
        }

        [Test]
        public void GetAllWrongUserId()
        {
            var result = categoryService.Get(wrongUserId);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(0, result.Response.Count());
        }

        [Test]
        public async Task Get()
        {
            var result = await categoryService.Get(userId, categoryId);            
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(categoryId, result.Response.Id);
            Assert.AreEqual(monthlyLimit, result.Response.MonthlyLimit);
            Assert.AreEqual(title, result.Response.Title);
        }

        [Test]
        public async Task GetWrongUserId()
        {
            var result = await categoryService.Get(wrongUserId, categoryId);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Access Forbidden", result.Error.Message);           
        }

        [Test]
        public async Task GetWrongId()
        {
            var result = await categoryService.Get(userId, wrongCategoryId);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Category not Found", result.Error.Message);
        }

        [Test]
        public async Task Create()
        {
            var monthlyLimit = 100;
            var title = "Create";
            var model = new CreateCategoryModel
            {
                MonthlyLimit = monthlyLimit,
                Title = title
            };
            var result = await categoryService.Create(userId, model);
            Assert.IsTrue(result.Succeeded);            
            Assert.AreEqual(monthlyLimit, result.Response.MonthlyLimit);
            Assert.AreEqual(title, result.Response.Title);
        }

        [Test]
        public async Task CreateWrongUserId()
        {
            var monthlyLimit = 100;
            var title = "Create";
            var model = new CreateCategoryModel
            {
                MonthlyLimit = monthlyLimit,
                Title = title
            };
            var result = await categoryService.Create(wrongUserId, model);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Creation Failed", result.Error.Message);
        }

        [Test]
        public async Task Update()
        {
            var monthlyLimit = 100;
            var title = "Update";
            var model = new CategoryModel
            {
                Id = categoryId,
                MonthlyLimit = monthlyLimit,
                Title = title
            };
            var result = await categoryService.Update(userId, model);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(categoryId, result.Response.Id);
            Assert.AreEqual(monthlyLimit, result.Response.MonthlyLimit);
            Assert.AreEqual(title, result.Response.Title);
        }

        [Test]
        public async Task UpdateWrongUserId()
        {
            var monthlyLimit = 100;
            var title = "Update";
            var model = new CategoryModel
            {
                Id = categoryId,
                MonthlyLimit = monthlyLimit,
                Title = title
            };
            var result = await categoryService.Update(wrongUserId, model);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Access Forbidden", result.Error.Message);
        }

        [Test]
        public async Task UpdateWrongId()
        {
            var monthlyLimit = 100;
            var title = "Update";
            var model = new CategoryModel
            {
                Id = wrongCategoryId,
                MonthlyLimit = monthlyLimit,
                Title = title
            };
            var result = await categoryService.Update(userId, model);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Category not Found", result.Error.Message);
        }

        [Test]
        public async Task Delete()
        {            
            var result = await categoryService.Delete(userId, categoryId);
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public async Task DeleteWrongId()
        {
            var result = await categoryService.Delete(userId, wrongCategoryId);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Category not Found", result.Error.Message);
        }

        [Test]
        public async Task DeleteWrongUserId()
        {
            var result = await categoryService.Delete(wrongUserId, categoryId);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Access Forbidden", result.Error.Message);
        }

        [Test]
        public async Task DeleteRejected()
        {
            context.Records.Add(new Record
            {
                CategoryId = categoryId,
                UserId = userId,
                Amount = 0,
                Description = "",
                IsIncome = true
            });
            context.SaveChanges();
            var result = await categoryService.Delete(userId, categoryId);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Delete Rejected", result.Error.Message);
        }

        [TearDown]
        public override void Dispose()
        {
            base.Dispose();
        }
        
    }
}
