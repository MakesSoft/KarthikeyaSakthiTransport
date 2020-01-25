namespace KarthikeyasakthiTransport.Model
{
    public class LedgerItemMaster
    {
        public int LedgerItemMasterId { get; set; }

        public int ItemMasterId { get; set; }

        public string ItemMasterName { get; set; }

        public int? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string BarCode { get; set; }

        public decimal? PurchaseRate { get; set; }
        public decimal? MaximumRetailPrice { get; set; }

        public decimal? SaleRate { get; set; }
        public decimal? WholeSaleRate { get; set; }
    }
}