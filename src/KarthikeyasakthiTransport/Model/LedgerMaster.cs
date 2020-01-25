using System;
using System.ComponentModel.DataAnnotations;

namespace KarthikeyasakthiTransport.Model
{
    public class LedgerMaster
    {
        public int LedgerMasterId { get; set; }

        [Required]
        public string LedgerMasterName { get; set; }

        public string LedgerMasterDescription { get; set; }
        public string Division { get; set; }
        public int SubGroupId { get; set; }
        public string SubGroupName { get; set; }
        public int LedgerMasterType { get; set; }
        public string MarginPercentage { get; set; }
        public string SaleRate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? PinCode { get; set; }
        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string GSTNumber { get; set; }
        public string PANNumber { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}