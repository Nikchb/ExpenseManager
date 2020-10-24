using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Models.AuthModels
{
    public class SignUpModel : SignModel
    {
        [Required]
        public string Name { get; set; }
    }
}
