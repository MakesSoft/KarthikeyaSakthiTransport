using System;
using System.ComponentModel.DataAnnotations;

namespace KarthikeyasakthiTransport.Model
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Required Category Name")]
        public string CategoryName { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}