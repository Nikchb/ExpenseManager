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

namespace ServerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        public readonly UserManager<User> userManager;
        public readonly IMapper mapper;

        public UserController(UserManager<User> userManager, IMapper mapper)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<UserModel>> Get()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if(user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            return Ok(mapper.Map<User,UserModel>(user));
        }

        [HttpPut]
        public async Task<ActionResult<UserModel>> Put([FromBody] UserModel model)
        {
            if (model == null || ModelState.IsValid == false)
            {
                return new BadRequestObjectResult(new { Message = "Model not Valid" });
            }

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            mapper.Map(model, user);

            await userManager.UpdateAsync(user);

            return Ok(mapper.Map<User, UserModel>(user));
        }
    }
}
