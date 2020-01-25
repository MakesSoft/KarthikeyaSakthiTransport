using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarthikeyasakthiTransport.Model
{
    [Table("GroupMaster")]
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Required Group Name")]
        public string GroupName { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }
    }
}