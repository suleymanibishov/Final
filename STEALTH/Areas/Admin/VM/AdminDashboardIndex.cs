using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Areas.Admin.VM
{
    public class UserVM
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class AdminDashboardIndex
    {
        public List<UserVM> Users { get; set; }
    }
}
