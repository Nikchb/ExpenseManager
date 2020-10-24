using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServerApp.Data.Models;
using ServerApp.Models;
using ServerApp.Models.AuthModels;
using ServerApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.AuthService
{
    public class AuthService : ServiceBase<string>, IAuthService
    {
        private readonly TokenGenerator tokenGenerator;
        private readonly UserManager<User> userManager;

        public AuthService(TokenGenerator tokenGenerator, UserManager<User> userManager)
        {
            this.tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<ServiceResponse<string>> SignIn(SignModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                return Error("User not Found!");
            }

            if (await userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                return Error("Sing In Failed");
            }

            var token = tokenGenerator.GenerateToken(user.Id, await userManager.GetRolesAsync(user));

            return Success(token);
        }

        public async Task<ServiceResponse<string>> SignUp(SignUpModel model)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Bill = 10000,
                Lang = "ru-RU"
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded == false)
            {
                var errors = new Dictionary<string, string>();
                foreach (IdentityError error in result.Errors)
                {
                    errors.Add(error.Code, error.Description);
                }
                return Error("Sing Up Failed", errors);
            }

            var token = tokenGenerator.GenerateToken(user.Id, await userManager.GetRolesAsync(user));

            return Success(token);
        }
    }
}
