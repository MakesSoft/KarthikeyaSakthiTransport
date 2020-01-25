using System;
using System.ComponentModel.DataAnnotations;

namespace KarthikeyasakthiTransport.Model
{
    public class SalesBill
    {
        public int SalesBillId { get; set; }
        public string InvoiceNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? SalesBillDate { get; set; }

        public int? ConsignorId { get; set; }
        public int? ConsigneeId { get; set; }
        public int? Value { get; set; }
        public string ModeOfPay { get; set; }
        public string LRNo { get; set; }
        public string EWayBillNo { get; set; }
        public string LorryNo { get; set; }
        public string DispatchThrough { get; set; }
        public string PaymentMode { get; set; }
        public string BillType { get; set; }
        public string DeliveryAt { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalWeight { get; set; }
        public decimal? DoorDeliveryCharge { get; set; }
        public decimal? AssableValue { get; set; }
        public decimal? GstPercentage { get; set; }
        public decimal? GstAmount { get; set; }
        public decimal? LoadingExpenses { get; set; }
        public decimal? RoundOff { get; set; }
        public decimal? GrandTotal { get; set; }
        public decimal? PreviousBalance { get; set; }
        public decimal? CurrentBalance { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}