using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServerApp.Data.Models;
using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly TokenGenerator tokenGenerator;
        private readonly UserManager<User> userManager;

        public AuthService(TokenGenerator tokenGenerator, UserManager<User> userManager)
        {
            this.tokenGenerator = tokenGenerator ?? throw new ArgumentNullException(nameof(tokenGenerator));
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<ServiceResponse> SignIn(SignModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                return new NotSucceededServiceResponse(new { Message = "User not Found!" });
            }

            if (await userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                return new NotSucceededServiceResponse(new { Message = "Sing In Failed" });
            }

            var token = tokenGenerator.GenerateToken(user.Id, await userManager.GetRolesAsync(user));

            return new SucceededServiceResponse(new { Token = token, Message = "Sign In Successful" });
        }

        public async Task<ServiceResponse> SignUp(SignUpModel model)
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
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }
                return new NotSucceededServiceResponse(new { Message = "Sing Up Failed", Errors = dictionary });
            }

            var token = tokenGenerator.GenerateToken(user.Id, await userManager.GetRolesAsync(user));

            return new SucceededServiceResponse(new { Token = token, Message = "Sign Up Successful" });
        }
    }
}
