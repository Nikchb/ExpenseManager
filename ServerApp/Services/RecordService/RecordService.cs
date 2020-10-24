using AutoMapper;
using ServerApp.Data;
using ServerApp.Data.Models;
using ServerApp.Models.RecordModels;
using ServerApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.RecordService
{
    public class RecordService : ServiceBase<RecordModel>, IRecordService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public RecordService(AppDbContext context, IMapper mapper)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public ServiceResponse<IEnumerable<RecordModel>> Get(string userId, RecordsFilterModel model)
        {
            var records = context.Records.Where(v => v.UserId == userId && v.Date >= model.StartDate && v.Date <= model.EndDate);
            if(model.CategoryId != null)
            {
                records = records.Where(v => v.CategoryId == model.CategoryId);
            }
            return Success(records.Select(v => mapper.Map<Record, RecordModel>(v)));
        }
        public async Task<ServiceResponse<RecordModel>> Get(string userId, string recordId)
        {
            var record = await context.Records.FindAsync(recordId);
            if (record == null)
            {
                return Error("Category not Found");
            }
            if (record.UserId != userId)
            {
                return Error("Access Forbidden");
            }            
            return Success(mapper.Map<Record, RecordModel>(record));
        }
        public async Task<ServiceResponse<RecordModel>> Create(string userId, CreateRecordModel model)
        {
            var category = await context.Categories.FindAsync(model.CategoryId);
            if (category == null)
            {
                return Error("Category not Found");
            }
            if (category.UserId != userId)
            {
                return Error("Access Forbidden");
            }
            var record = new Record();
            mapper.Map<CreateRecordModel, Record>(model);
            record.UserId = userId;
            try
            {
                await context.Records.AddAsync(record);
                await context.SaveChangesAsync();
            }
            catch
            {
                return Error("Creation Failed");
            }
            return Success(mapper.Map<Record, RecordModel>(record));
        }

               
        public async Task<ServiceResponse<RecordModel>> Update(string userId, UpdateRecordModel model)
        {
            var record = await context.Records.FindAsync(model.Id);
            if (record == null)
            {
                return Error("Record not Found");
            }
            var category = await context.Categories.FindAsync(model.CategoryId);
            if (category == null)
            {
                return Error("Category not Found");
            }
            if (category.UserId != userId || record.UserId != userId)
            {
                return Error("Access Forbidden");
            }            
            mapper.Map(model, record);
            record.UserId = userId;
            try
            {                
                await context.SaveChangesAsync();
            }
            catch
            {
                return Error("Update Failed");
            }
            return Success(mapper.Map<Record, RecordModel>(record));
        }

        public async Task<ServiceResponse<RecordModel>> Delete(string userId, string recordId)
        {
            var record = await context.Records.FindAsync(recordId);
            if (record == null)
            {
                return Error("Record not Found");
            }            
            if (record.UserId != userId)
            {
                return Error("Access Forbidden");
            }            
            try
            {
                context.Records.Remove(record);
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
