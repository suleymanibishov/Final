using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Areas.Admin.VM
{
    public class RoleVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Role is Empity")]
        public string Name { get; set; }
    }
}
