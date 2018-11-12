using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaapApp.Models
{
    public class ReportPartReceive
    {
        public Int64 Id { get; set; }
        public DateTime DateToProduction { get; set; }
        public string CustomEntryNo { get; set; }
        public string InvoiceNo { get; set; }
        public string ReceiveNo { get; set; }
        public string Model { get; set; }
        public string PartNo { get; set; }
        public string PartDescription { get; set; }
        public int Qty { get; set; }
        public int Qpv { get; set; }
        public int Amount { get; set; }
        public string Um { get; set; }
    }
}
