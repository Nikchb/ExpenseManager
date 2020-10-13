using ServerApp.Models;
using ServerApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.AuthService
{
    public interface IAuthService
    {
        public Task<ServiceResponse<string>> SignIn(SignModel model);
        public Task<ServiceResponse<string>> SignUp(SignUpModel model);
    }
}
