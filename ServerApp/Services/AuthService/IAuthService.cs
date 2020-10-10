using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.AuthService
{
    public interface IAuthService
    {
        public Task<ServiceResponse> SignIn(SignModel model);
        public Task<ServiceResponse> SignUp(SignUpModel model);
    }
}
