using System;
using System.ComponentModel.DataAnnotations;

namespace KarthikeyasakthiTransport.Model
{
    public class ItemMaster
    {
        public int ItemMasterId { get; set; }

        [Required(ErrorMessage = "Required Item Master Name")]
        public string ItemMasterName { get; set; }

        public string ItemMasterDescription { get; set; }
        public string ItemMasterNameTamil { get; set; }

        [Required(ErrorMessage = "Please select valid category")]
        public int? CategoryId { get; set; }

        //public string CategoryName { get; set; }

        [Required(ErrorMessage = "Required BarCode Name")]
        public string BarCode { get; set; }

        public int Case { get; set; }
        public int? Quantity { get; set; }
        public string HsnCode { get; set; }

        public int? UnitId { get; set; }

        public decimal? MaximumRetailPrice { get; set; }
        public decimal? BasicRate { get; set; }
        public decimal? SaleRate { get; set; }
        public decimal? WholeSaleRate { get; set; }
        public decimal? PurchaseRate { get; set; }
        public decimal? TaxPercentage { get; set; }
        public bool GSTInclude { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? BeforeTax { get; set; }
        public decimal? AfterTax { get; set; }
        public decimal? MarkDownPercentage { get; set; }
        public decimal? MarkDownAmount { get; set; }
        public decimal? MarkUpPercentage { get; set; }
        public decimal? MarkUpAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? CessPercentage { get; set; }
        public decimal? AdditionalCess { get; set; }
        public int? ReorderLevel { get; set; }
        public decimal? OpeningStock { get; set; }
        public decimal? CurrentStock { get; set; }
        public bool StockMaintenance { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}