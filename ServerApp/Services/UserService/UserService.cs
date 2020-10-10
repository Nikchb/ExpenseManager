using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServerApp.Data.Models;
using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ServiceResponse> Get(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new NotSucceededServiceResponse(new { Message = "User not found" });
            }
            return new SucceededServiceResponse(mapper.Map<User, UserModel>(user));
        }

        public async Task<ServiceResponse> Update(string userId, UserModel model)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new NotSucceededServiceResponse(new { Message = "User not found" });
            }

            mapper.Map(model, user);

            var result = await userManager.UpdateAsync(user);
            if(result.Succeeded == false)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }
                return new NotSucceededServiceResponse(new { Message = "Update Failed", Errors = dictionary });
            }

            return new SucceededServiceResponse(mapper.Map<User, UserModel>(user));
        }
    }
}
