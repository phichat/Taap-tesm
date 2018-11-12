using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaapApp.Models
{
    public partial class MasterModels
    {
        public int ModelId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int TypeId { get; set; }
        [Required]
        public string ModelName { get; set; }
        [Required]
        public bool Status { get; set; }
        public string CreateBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreateDate { get; set; }

        public string UpdateBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? UpdateDate { get; set; }
    }

    public class ViewMasterModels
    {
        public int ModelId { get; set; }
        public string TypeName { get; set; }
        public string ModelName { get; set; }
        public bool Status { get; set; }
    }
}
