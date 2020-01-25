using System;
using System.ComponentModel.DataAnnotations;

namespace KarthikeyasakthiTransport.Model
{
    public class SubGroup
    {
        [Key]
        public int SubGroupId { get; set; }

        [Required(ErrorMessage = "Required SubGroup Name")]
        public string SubGroupName { get; set; }

        [Required(ErrorMessage = "Please select valid Group Name")]
        public int? GroupId { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}