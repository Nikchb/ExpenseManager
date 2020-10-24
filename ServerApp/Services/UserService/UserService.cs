using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServerApp.Data.Models;
using ServerApp.Models;
using ServerApp.Models.UserModels;
using ServerApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.UserService
{
    public class UserService : ServiceBase<UserModel>, IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResponse<UserModel>> Get(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Error("User not found");
            }
            return Success(mapper.Map<User, UserModel>(user));
        }

        public async Task<ServiceResponse<UserModel>> Update(string userId, UserModel model)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Error("User not found");                
            }

            mapper.Map(model, user);

            var result = await userManager.UpdateAsync(user);
            if(result.Succeeded == false)
            {
                var errors = new Dictionary<string, string>();
                foreach (IdentityError error in result.Errors)
                {
                    errors.Add(error.Code, error.Description);
                }
                return Error("Update Failed", errors);
            }
            return Success(mapper.Map<User, UserModel>(user));
        }
    }
}
