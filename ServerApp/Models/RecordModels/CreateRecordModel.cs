using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Models.RecordModels
{
    public class CreateRecordModel
    {
        [Required]
        public string CategoryId { get; set; }

        [Required]
        public decimal Amount { get; set; }        

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsIncome { get; set; }
    }
}
