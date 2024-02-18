using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<BasketProduct> BasketProducts { get; set; }
    }
}
