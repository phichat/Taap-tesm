using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaapApp.Models
{
    public class ReportCarMovement
    {
        public Int64 Id { get; set; }
        public DateTime DateFG { get; set; }
        public string CommissionNo { get; set; }
        public string Model { get; set; }
        public string PackingMonth { get; set; }
        public string SetNo { get; set; }
        public int Unit { get; set; }
        public DateTime DateBuyOff { get; set; }
        public int Amount { get; set; }
    }
}
