using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaapApp.Models
{
    public partial class MasterTypeItem
    {
        public int ItemId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int TypeId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int ShopId { get; set; }
        [Required]
        public bool Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class ViewMasterTypeItem
    {
        public int ItemId { get; set; }
        public string TypeName { get; set; }
        public string ShopName { get; set; }
        public bool Status { get; set; }
    }

    public partial class MasterTypeItemDetail
    {
        public int ItemId { get; set; }
        public string TypeName { get; set; }
        public string ShopName { get; set; }
        public bool Status { get; set; }
        public string CreateBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? UpdateDate { get; set; }
    }
}
