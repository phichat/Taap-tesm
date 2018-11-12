using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaapApp.Models
{
    public class Permissions
    {
        public int PermissionId { get; set; }
        [Required]
        public int RoleId { get; set; }
        [Required]
        public string Form { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Viewer { get; set; }
        [Required]
        public bool Creater { get; set; }
        [Required]
        public bool Editer { get; set; }
        [Required]
        public bool Deleter { get; set; }
        [Required]
        public bool Printer { get; set; }

    }

    public class PermissionsViewModel
    {
        public int PermissionId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Form { get; set; }
        public string Description { get; set; }
        public string Viewer { get; set; }
        public string Creater { get; set; }
        public string Editer { get; set; }
        public string Deleter { get; set; }
        public string Printer { get; set; }
    }
}
