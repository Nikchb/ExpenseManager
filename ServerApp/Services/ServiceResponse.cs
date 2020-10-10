using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services
{
    public class ServiceResponse
    {
        public ServiceResponse()
        { 
            
        }      

        public ServiceResponse(bool succeeded, object response)
        {
            Succeeded = succeeded;
            Response = response;
        }

        public bool Succeeded { get; set; }
        public object Response { get; set; }
    }

    public class NotSucceededServiceResponse : ServiceResponse
    {
        public NotSucceededServiceResponse()
        {
            Succeeded = false;
        }

        public NotSucceededServiceResponse(object response)
        {
            Succeeded = false;
            Response = response;
        }
    }

    public class SucceededServiceResponse : ServiceResponse
    {
        public SucceededServiceResponse()
        {
            Succeeded = true;
        }

        public SucceededServiceResponse(object response)
        {
            Succeeded = true;
            Response = response;
        }
    }
}
