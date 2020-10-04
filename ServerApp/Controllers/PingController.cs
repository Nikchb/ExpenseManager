using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Data.Models;

namespace ServerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PingController : ControllerBase
    {       
        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public IActionResult GetAnonymous()
        {
            return Ok("ping");
        }

        [HttpGet]
        [Route("authorize")]        
        public IActionResult GetAuthorize()
        {            
            return Ok($"ping {User.Identity.Name}");
        }
    }
}
