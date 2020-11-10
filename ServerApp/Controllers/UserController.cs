using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Data.Models;
using ServerApp.Models;
using ServerApp.Models.UserModels;
using ServerApp.Services.Models;
using ServerApp.Services.UserService;

namespace ServerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : CustomControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var result = await userService.Get(UserId);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ServiceError), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromBody] UpdateUserModel model)
        {
            var result = await userService.Update(UserId, model);
            if (result.Succeeded)
            {
                return Ok(result.Response);
            }
            return BadRequest(result.Response);
        }
    }
}
