using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServerApp.Controllers
{
    [Authorize]
    public abstract class CustomControllerBase : ControllerBase
    {
        public string UserId => User.FindFirst("UserId").Value;
    }
}
