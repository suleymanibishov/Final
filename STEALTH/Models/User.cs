using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Models
{
    public class User:IdentityUser<int>
    {
        public Basket Basket { get; set; }
    }
}
