using System.ComponentModel.DataAnnotations;

namespace KarthikeyasakthiTransport.Model
{
    public class MenuMaster

    {
        [Key]
        public int MenuId { get; set; }

        public string MenuName { get; set; }

        public int ParentId { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string QueryString { get; set; }

        public int OrderBy { get; set; }

        public int SubMenu { get; set; }
        public bool Visible { get; set; }
    }
}