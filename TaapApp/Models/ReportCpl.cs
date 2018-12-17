using System;
namespace TaapApp.Models
{
    public class ReportCpl
    {
        public Int64 Id { get; set; }
        public DateTime DateFG { get; set; }
        public string CommissionFrom { get; set; }
        public string CommissionTo { get; set; }
        public string Model { get; set; }
        public string PackingMonth { get; set; }
        public string SetNo { get; set; }
    }
}
