using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.VM
{
    public class RegisterVM
    {
        [Required] public string UserName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Password { get; set; }
        [Required][Compare(nameof(Password))]
        public string ConfirPassword { get; set; }
    }
}
