using STEALTH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.VM
{
    public class ShopVM
    {
        public int PageCount { get; set; }

        public int Page { get; set; }

        public string Categori { get; set; }

        public List<CategoryProduct> Categories { get; set; }

        public List<Product> Products { get; set; }
    }
}
