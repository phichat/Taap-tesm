using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaapApp.Models
{
    public partial class MasterTypes
    {
        public int TypeId { get; set; }
        [Required]
        public string TypeName { get; set; }
        [Required]
        public bool Status { get; set; }
        public string CreateBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? UpdateDate { get; set; }
    }
}
