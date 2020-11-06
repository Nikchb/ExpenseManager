using NUnit.Framework;
using ServerApp.Data.Models;
using ServerApp.Models.RecordModels;
using ServerApp.Services.RecordService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApp.Tests.Services
{
    [TestFixture]
    public class RecordServiceTest : BaseTest
    {
        private readonly string userId = "1";

        private readonly string wrongUserId = "2";

        private readonly string categoryId = "1";        

        private readonly string wrongCategoryId = "2";

        private readonly string recordId = "1";

        private readonly string wrongRecordId = "2";

        private readonly decimal amount = 10000;
        private readonly string description = "Description";
        private readonly bool isIncome = true;

        private IRecordService recordService;

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
                Bill = amount,
                Lang = "ru-RU"
            });
            context.Categories.Add(new Category
            {
                Id = categoryId,
                UserId = userId,
                MonthlyLimit = 1000,
                Title = "Test"
            });
            context.Records.Add(new Record
            {
                Id = recordId,
                UserId = userId,
                CategoryId = categoryId,
                Amount = amount,
                Description = description,
                IsIncome = isIncome
            });
            context.SaveChanges();
            recordService = new RecordService(context, mapper);
        }

        [Test]
        public void GetAll()
        {
            var result = recordService.Get(userId, new RecordsFilterModel { CategoryId = categoryId });
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(1, result.Response.Count());
            Assert.IsTrue(
                result.Response.Any(v =>
                    v.Id == recordId &&
                    v.Amount == amount &&
                    v.Description == description &&
                    v.IsIncome == isIncome
                )
           );
        }

        [Test]
        public void GetAllWrongUserId()
        {
            var result = recordService.Get(wrongUserId, new RecordsFilterModel { CategoryId = categoryId });
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(0, result.Response.Count());
        }

        [Test]
        public void GetAllWrongCategoryId()
        {
            var result = recordService.Get(userId, new RecordsFilterModel { CategoryId = wrongCategoryId });
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(0, result.Response.Count());
        }

        [Test]
        public async Task Get()
        {
            var result = await recordService.Get(userId, recordId);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(amount, result.Response.Amount);
            Assert.AreEqual(description, result.Response.Description);
            Assert.AreEqual(isIncome, result.Response.IsIncome);
            Assert.AreEqual(recordId, result.Response.Id);
        }

        [Test]
        public async Task GetWrongUserId()
        {
            var result = await recordService.Get(wrongUserId, recordId);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Access Forbidden", result.Error.Message);
        }

        [Test]
        public async Task GetWrongId()
        {
            var result = await recordService.Get(userId, wrongRecordId);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Record not Found", result.Error.Message);
        }

        [Test]
        public async Task Create()
        {            
            var amount = 9000;
            var description = "Create";
            var isIncome = false;            
            var model = new CreateRecordModel
            {
                Amount = amount,
                Description = description,
                IsIncome = isIncome,
                CategoryId = categoryId
            };
            var result = await recordService.Create(userId, model);
            Assert.AreEqual(amount, result.Response.Amount);
            Assert.AreEqual(description, result.Response.Description);
            Assert.AreEqual(isIncome, result.Response.IsIncome);            
        }

        [Test]
        public async Task CreateWrongUserId()
        {
            var amount = 9000;
            var description = "Create";
            var isIncome = false;
            var model = new CreateRecordModel
            {
                Amount = amount,
                Description = description,
                IsIncome = isIncome,
                CategoryId = categoryId
            };           
            var result = await recordService.Create(wrongUserId, model);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Access Forbidden", result.Error.Message);
        }

        [Test]
        public async Task CreateWrongCategoryId()
        {
            var amount = 9000;
            var description = "Create";
            var isIncome = false;
            var model = new CreateRecordModel
            {
                Amount = amount,
                Description = description,
                IsIncome = isIncome,
                CategoryId = wrongCategoryId
            };
            var result = await recordService.Create(userId, model);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Category not Found", result.Error.Message);
        }

        [Test]
        public async Task CreateNotEnoughMoney()
        {
            var amount = 11000;
            var description = "Create";
            var isIncome = false;
            var model = new CreateRecordModel
            {
                Amount = amount,
                Description = description,
                IsIncome = isIncome,
                CategoryId = categoryId                
            };
            var result = await recordService.Create(userId, model);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Not enough money", result.Error.Message);
        }

        [Test]
        public async Task Update()
        {
            var amount = 11000;
            var description = "Update";
            var isIncome = true;
            var model = new UpdateRecordModel
            {
                Id = recordId,
                Amount = amount,
                Description = description,
                IsIncome = isIncome,
                CategoryId = categoryId
            };
            var result = await recordService.Update(userId, model);
            Assert.IsTrue(result.Succeeded);
            Assert.AreEqual(amount, result.Response.Amount);
            Assert.AreEqual(description, result.Response.Description);
            Assert.AreEqual(isIncome, result.Response.IsIncome);
            Assert.AreEqual(recordId, result.Response.Id);
        }

        [Test]
        public async Task UpdateWrongUserId()
        {
            var amount = 11000;
            var description = "Update";
            var isIncome = true;
            var model = new UpdateRecordModel
            {
                Id = recordId,
                Amount = amount,
                Description = description,
                IsIncome = isIncome,
                CategoryId = categoryId
            };
            var result = await recordService.Update(wrongUserId, model);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Access Forbidden", result.Error.Message);
        }

        [Test]
        public async Task UpdateWrongCategoryId()
        {
            var amount = 11000;
            var description = "Update";
            var isIncome = true;
            var model = new UpdateRecordModel
            {
                Id = recordId,
                Amount = amount,
                Description = description,
                IsIncome = isIncome,
                CategoryId = wrongCategoryId
            };
            var result = await recordService.Update(userId, model);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Category not Found", result.Error.Message);
        }

        [Test]
        public async Task UpdateWrongId()
        {
            var amount = 11000;
            var description = "Update";
            var isIncome = true;
            var model = new UpdateRecordModel
            {
                Id = wrongRecordId,
                Amount = amount,
                Description = description,
                IsIncome = isIncome,
                CategoryId = categoryId
            };
            var result = await recordService.Update(userId, model);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Record not Found", result.Error.Message);
        }

        [Test]
        public async Task UpdateNotEnoughMoneyOne()
        {            
            var amount = 10000;
            var description = "Update";
            var isIncome = false;
            var model = new UpdateRecordModel
            {
                Id = recordId,
                Amount = amount,
                Description = description,
                IsIncome = isIncome,
                CategoryId = categoryId
            };
            var result = await recordService.Update(userId, model);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Not enough money", result.Error.Message);
        }

        [Test]
        public async Task UpdateNotEnoughMoneyTwo()
        {
            await context.AddAsync(new Record
            {                
                UserId = userId,
                CategoryId = categoryId,
                Amount = 9000,
                Description = "",
                IsIncome = false
            });
            var user = await context.Users.FindAsync(userId);
            user.Bill = 1000;
            await context.SaveChangesAsync();

            var amount = 8000;
            var description = "Update";
            var isIncome = true;
            var model = new UpdateRecordModel
            {
                Id = recordId,
                Amount = amount,
                Description = description,
                IsIncome = isIncome,
                CategoryId = categoryId
            };
            var result = await recordService.Update(userId, model);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Not enough money", result.Error.Message);            
        }

        [Test]
        public async Task Delete()
        {
            var result = await recordService.Delete(userId, recordId);
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        public async Task DeleteWrongId()
        {
            var result = await recordService.Delete(userId, wrongRecordId);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Record not Found", result.Error.Message);
        }

        [Test]
        public async Task DeleteWrongUserId()
        {            
            var result = await recordService.Delete(wrongUserId, recordId);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Access Forbidden", result.Error.Message);
        }

        [Test]
        public async Task DeleteNotEnoughMoney()
        {
            await context.AddAsync(new Record
            {                
                UserId = userId,
                CategoryId = categoryId,
                Amount = 9000,
                Description = "",
                IsIncome = false
            });
            var user = await context.Users.FindAsync(userId);
            user.Bill = 1000;
            await context.SaveChangesAsync();


            var result = await recordService.Delete(userId, recordId);
            Assert.IsFalse(result.Succeeded);
            Assert.AreEqual("Not enough money", result.Error.Message);
        }       

        [TearDown]
        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
