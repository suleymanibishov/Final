﻿using STEALTH.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.VM
{
    public class LoginVM
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

    }
}
