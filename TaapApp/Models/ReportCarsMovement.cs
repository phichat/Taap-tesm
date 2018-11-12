using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaapApp.Models
{
    public class ReportCarsMovement
    {
        public DateTime? DateFG { get; set; }
        public string CommissionNo { get; set; }
        public string Model { get; set; }
        public string PackingMonth { get; set; }
        public string ReferenceNo { get; set; }
        public string Consignment { get; set; }
        public int QPV { get; set; }
        public DateTime? DateBuyOff { get; set; }
        public int Amount { get; set; }
    }
}
