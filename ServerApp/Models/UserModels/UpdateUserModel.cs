using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Models.UserModels
{
    public class UpdateUserModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Lang { get; set; }
    }
}
