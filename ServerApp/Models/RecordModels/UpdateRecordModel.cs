using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Models.RecordModels
{
    public class UpdateRecordModel : CreateRecordModel
    {
        [Required]
        public string Id { get; set; }        
    }
}
