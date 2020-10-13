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
using ServerApp.Services.Models;

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
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]        
        public async Task<IActionResult> SingUp([FromBody] SignUpModel model)
        {
            var result = await authService.SignUp(model);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Error);
        }

        [HttpPost]
        [Route("sign-in")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn([FromBody] SignModel model)
        {
            var result = await authService.SignIn(model);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Error);
        }
    }
}
