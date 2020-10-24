using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Models.RecordModels
{
    public class RecordModel : UpdateRecordModel
    {
        [Required]
        public DateTime Date { get; set; }
    }
}
