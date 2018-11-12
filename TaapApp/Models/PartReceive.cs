using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaapApp.Models
{
    public partial class PartReceive
    {
        public int ReceiveId { get; set; }
        [Required]
        public string ReceiveNo { get; set; }
        public string BuyOffRecNo { get; set; }
        [Required]
        public string PackingMonth { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Consignment { get; set; }
        [Required]
        public string CommissionFrom { get; set; }
        [Required]
        public string CommissionTo { get; set; }
        [Required]
        public string Shop { get; set; }
        [Required]
        public string CustomEntryNo { get; set; }
        [Required]
        public string InvoiceNo { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateToProduction { get; set; }
        [Required]
        public string PartNo { get; set; }
        [Required]
        public string PartDescription { get; set; }
        [Required]
        public int Qty { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public int Qpv { get; set; }
        [Required]
        public string Um { get; set; }
        [Required]
        public string PartType { get; set; }
    }

    public partial class PartReceiveSummary
    {
        public string ReceiveNo { get; set; }
        public string PackingMonth { get; set; }
        public string Model { get; set; }
        public string Consignment { get; set; }
        public string CommissionFrom { get; set; }
        public string CommissionTo { get; set; }
        public string Shop { get; set; }
        public int CountCommission { get; set; }
        public int TotalCount { get; set; }
        public int TotalQty { get; set; }

    }

}
