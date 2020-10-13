using Microsoft.AspNetCore.Mvc.Formatters;
using ServerApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services
{
    public class ServiceBase<T>
    {
        public ServiceResponse<T> Success()
        {
            return new ServiceResponse<T>(true);
        }

        public ServiceResponse<T> Success(T response)
        {
            return new ServiceResponse<T>(response);
        }

        public ServiceResponse<IEnumerable<T>> Success(IEnumerable<T> response)
        {
            return new ServiceResponse<IEnumerable<T>>(response);
        }

        public ServiceResponse<T> Error()
        {
            return new ServiceResponse<T>(false);
        }

        public ServiceResponse<T> Error(ServiceError error)
        {
            return new ServiceResponse<T>(error);
        }

        public ServiceResponse<T> Error(string messsage)
        {
            return new ServiceResponse<T>(new ServiceError { Message = messsage });
        }

        public ServiceResponse<T> Error(string messsage, Dictionary<string, string> errors)
        {
            return new ServiceResponse<T>(new ServiceError { Message = messsage, Errors = errors });
        }
    }
}
