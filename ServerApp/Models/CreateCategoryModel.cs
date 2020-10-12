using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServerApp.Models
{
    public class CreateCategoryModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [JsonPropertyName("limit")]
        public decimal MonthlyLimit { get; set; }
    }
}
