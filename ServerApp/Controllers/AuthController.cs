using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServerApp.Data.Models;
using ServerApp.Models;
using ServerApp.Services;

namespace ServerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly TokenGenerator _tokenGenerator;
        private readonly UserManager<User> _userManager;

        public AuthController(TokenGenerator tokenGenerator, UserManager<User> userManager)
        {
            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("sign-up")]
        public async Task<IActionResult> SingUp([FromBody] SignUpModel model)
        {
            if (model == null || ModelState.IsValid == false)
            {
                return new BadRequestObjectResult(new { Message = "Model not Valid" });
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Bill = 10000,
                Lang = "ru-RU"
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded == false)
            {
                var dictionary = new ModelStateDictionary();
                foreach (IdentityError error in result.Errors)
                {
                    dictionary.AddModelError(error.Code, error.Description);
                }
                return new BadRequestObjectResult(new { Message = "Sing Up Failed", Errors = dictionary });
            }
            
            var token = _tokenGenerator.GenerateToken(user.UserName, await _userManager.GetRolesAsync(user));
           
            return Ok(new { Token = token, Message = "Sign Up Successful" });
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignModel model)
        {
            if (model == null || ModelState.IsValid == false)
            {
                return new BadRequestObjectResult(new { Message = "Model not Valid" });
            }

            var user = await _userManager.FindByNameAsync(model.Email);

            if (user == null)
            {
                return new NotFoundObjectResult(new { Message = "User not Found!" });
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password) == false)
            {
                return new BadRequestObjectResult(new { Message = "Sing In Failed" });
            }

            var token = _tokenGenerator.GenerateToken(user.UserName, await _userManager.GetRolesAsync(user));
            return Ok(new { Token = token, Message = "Sign In Successful" });
        }
    }
}
