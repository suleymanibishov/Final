using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Models
{
	public class ProductImage
	{
		public int Id { get; set; }
		public string ImagePath { get; set; }

		public int ProductId { get; set; }
		public Product Product { get; set; }
	}
}
