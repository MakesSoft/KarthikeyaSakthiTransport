using System;

namespace KarthikeyasakthiTransport.Model
{
    public class SalesBillItems
    {
        public int SalesBillItemsId { get; set; }
        public int SalesBillId { get; set; }
        public int ItemMasterId { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public decimal Weight { get; set; }
        public decimal Amount { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}