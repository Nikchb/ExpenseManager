using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServerApp.Data.Models;
using ServerApp.Models;
using ServerApp.Services;
using ServerApp.Services.AuthService;

namespace ServerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost]
        [Route("sign-up")]
        public async Task<IActionResult> SingUp([FromBody] SignUpModel model)
        {
            var result = await authService.SignUp(model);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Response);
        }

        [HttpPost]
        [Route("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignModel model)
        {
            var result = await authService.SignIn(model);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Response);
        }
    }
}
