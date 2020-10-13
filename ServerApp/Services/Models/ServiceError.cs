using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services.Models
{
    public class ServiceError
    {
        public string Message { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }
}
