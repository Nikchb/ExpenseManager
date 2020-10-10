using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Models
{
    public class CategoryModel : CreateCategoryModel
    {        
        [Required]
        public string Id { get; set; }
    }
}
