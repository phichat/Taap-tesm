using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaapApp.Models
{
    public class ReportPartsMovement
    {
        public DateTime DateToProduction { get; set; }
        public string ReferenceNo { get; set; }
        public string ReceiveNo { get; set; }
        public string PartNo { get; set; }
        public string PartDescription { get; set; }
        public int Qty { get; set; }
        public int QPV { get; set; }
        public string CommissionNo { get; set; }
        public DateTime? DateFG { get; set; }
        public DateTime? DateBuyOff { get; set; }
        public string VDONo { get; set; }
        public int Amount { get; set; }
    }
}
