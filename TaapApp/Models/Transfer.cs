using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaapApp.Models
{
    public class Transfer
    {
        public int TfId { get; set; }
        public DateTime DateToProduction { get; set; }
        public string Model { get; set; }
        public string PackingMonth { get; set; }
        public string Consignment { get; set; }
        public string PartType { get; set; }
        public string TfNo { get; set; }
    }

    public class TransferResponse
    {
        public int? TfId { get; set; }
        public DateTime DateToProduction { get; set; }
        public int Status { get; set; }
        public string StatusDesc { get; set; }
        public string Model { get; set; }
        public string PackingMonth { get; set; }
        public string Consignment { get; set; }
        public string CommissionTo { get; set; }
        public string CommissionFrom { get; set; }
        public string PartType { get; set; }
        public string TfNo { get; set; }
    }

}
