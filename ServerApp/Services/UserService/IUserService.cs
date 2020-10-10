using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.UserService
{
    public interface IUserService
    {
        public Task<ServiceResponse> Get(string userId);

        public Task<ServiceResponse> Update(string userId, UserModel model);
    }
}
