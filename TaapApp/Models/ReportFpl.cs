using System;
namespace TaapApp.Models
{
    public class ReportFpl
    {
        public Int64 Id { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string SetNo { get; set; }
        public string CommissionFrom { get; set; }
        public string CommissionTo { get; set; }
        public string CommissionNo { get; set; }
    }
}
