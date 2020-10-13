using ServerApp.Models;
using ServerApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.UserService
{
    public interface IUserService
    {
        public Task<ServiceResponse<UserModel>> Get(string userId);

        public Task<ServiceResponse<UserModel>> Update(string userId, UserModel model);
    }
}
