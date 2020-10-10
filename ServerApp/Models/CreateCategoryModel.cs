using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models
{
    public class CreateCategoryModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public decimal MonthlyLimit { get; set; }
    }
}
