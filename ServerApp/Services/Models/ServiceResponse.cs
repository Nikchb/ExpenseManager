using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.Models
{
    public class ServiceResponse<T>
    {
        public bool Succeeded { get; set; }
        public T Response { get; set; }
        public ServiceError Error { get; set; }

        public ServiceResponse(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public ServiceResponse(ServiceError error)
        {
            Succeeded = false;
            Error = error;
        }

        public ServiceResponse(T response)
        {
            Succeeded = true;
            Response = response;
        }
    }    
}
