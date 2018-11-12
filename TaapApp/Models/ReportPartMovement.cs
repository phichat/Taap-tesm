using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaapApp.Models
{
    public class ReportPartMovement
    {
        public Int64 Id { get; set; }
        public DateTime DateToProduction { get; set; }
        public string TransferNo { get; set; }
        public string SetNo { get; set; }
        public string ReceiveNo { get; set; }
        public string Model { get; set; }
        public string PartNo { get; set; }
        public string PartDescription { get; set; }
        public int Qty { get; set; }
        public int Qpv { get; set; }
        public string CommissionNo { get; set; }
        public DateTime? DateFG { get; set; }
        public DateTime? DateBuyOff { get; set; }
        public string VDONo { get; set; }
        public int Amount { get; set; }
    }
}
