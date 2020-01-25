using System;
using System.ComponentModel.DataAnnotations;

namespace KarthikeyasakthiTransport.Model
{
    public class Unit
    {
        [Key]
        public int UnitId { get; set; }

        [Required(ErrorMessage = "Required Unit Name")]
        public string UnitName { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}