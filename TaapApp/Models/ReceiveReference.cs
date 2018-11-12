using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaapApp.Models
{
    public partial class ReceiveReference
    {
        public int Vdoid { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? Date { get; set; }
        [Required]
        public string Vdono { get; set; }
        [Required]
        public string ModelType { get; set; }
        public string LotNo { get; set; }
        public string SetNo { get; set; }
        public string ReferenceNo { get; set; }
        public string ProductionNo { get; set; }
        public string PaintTrim { get; set; }
        [Required]
        public string CommissionNo { get; set; }
        [Required]
        public string ReceiveNo { get; set; }         
        public string DateToProduction { get; set; }
        public string TransferNoCkd { get; set; }
        public string TransferNoLoc { get; set; }
        public string ChassisNo { get; set; }
        public string EngineNo { get; set; }
        public string Remark { get; set; }
        public int? Status { get; set; }
        public int? ComItem { get; set; }
    }

    public partial class ReceiveReferenceListView
    {
        public int Status { get; set; }
        public string StatusDesc { get; set; }
        public string DateToProduction { get; set; }
        public int Vdoid { get; set; }
        public string SetNo { get; set; }
        public string ReceiveNo { get; set; }
        public string CommissionNo { get; set; }
        public string TransferNoCkd { get; set; }
        public string TransferNoLoc { get; set; }
        public string Vdono { get; set; }
        public DateTime? Date { get; set; }
    }

    public partial class BuyOffViewModel
    {
        public int? Vdoid { get; set; }
        public string ReceiveNo { get; set; }
        public string CommissionNo { get; set; }
        public string TransferNoCkd { get; set; }
        public string TransferNoLoc { get; set; }
        public string Vdono { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BuyOffDate { get; set; }
        public int? Status { get; set; }
    }

    public partial class ReceiveReferenceSummary
    {
        public string Vdono { get; set; }
        public int inProcess { get; set; }
        public int notMatch { get; set; }
        public int isMatch { get; set; }
        public int TotalCount { get; set; }
    }
}
