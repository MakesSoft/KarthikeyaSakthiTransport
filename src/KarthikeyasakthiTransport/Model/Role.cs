using System.ComponentModel.DataAnnotations;

namespace KarthikeyasakthiTransport.Model
{
    public partial class Roles
    {
        [Key]
        public int RoleID { get; set; }

        public string Rolename { get; set; }
    }
}