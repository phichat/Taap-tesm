using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaapApp.Models
{
    public class ReportStockMaterial
    {
        public Int64 Id { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string ReceiveNo { get; set; }
        public string CustomEntryNo { get; set; }
        public string InvoiceNo { get; set; }
        public string CommissionFrom { get; set; }
        public string CommissionTo { get; set; }
        public string Model { get; set; }
        public string Shop { get; set; }
        public int Amount { get; set; }

    }
}
