using System.ComponentModel.DataAnnotations;

namespace KarthikeyasakthiTransport.Model
{
    public class ItemMasterViewModel
    {
        [Required]
        public string CategoryName { get; set; }

        public string UnitName { get; set; }

        public ItemMaster ItemMaster { get; set; }
    }
}