using ServerApp.Models.RecordModels;
using ServerApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.RecordService
{
    public interface IRecordService
    {
        ServiceResponse<IEnumerable<RecordModel>> Get(string userId, RecordsFilterModel model);
        Task<ServiceResponse<RecordModel>> Get(string userId, string recordId);
        Task<ServiceResponse<RecordModel>> Create(string userId, CreateRecordModel model);
        Task<ServiceResponse<RecordModel>> Update(string userId, UpdateRecordModel model);
        Task<ServiceResponse<RecordModel>> Delete(string userId, string recordId);
    }
}
