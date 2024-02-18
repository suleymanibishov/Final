﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Models
{
    public class CategoryProduct
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<CategoryProductBrig> CategoryProducts { get; set; }
    }
}
