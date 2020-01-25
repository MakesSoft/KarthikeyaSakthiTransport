using System;
using System.ComponentModel.DataAnnotations;

namespace KarthikeyasakthiTransport.Model
{
    public class Settings
    {
        [Key]
        public int SettingsId { get; set; }

        public string CustomBarCode { get; set; }
        public int? BarCodeStartFrom { get; set; }
        public bool GSTInclude { get; set; }
        public bool ItemSettings { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}