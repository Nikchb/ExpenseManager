using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Models.UserModels
{
    public class UserModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Bill { get; set; }
    }
}
