using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Areas.Admin.VM
{
    public class Categori
    {
        public int CategoriId { get; set; }
        public string Name { get; set; }
        public bool Exist { get; set; }
    }
    public class ProductVM
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Price { get; set; }

        private IFormFile _imageFile;
        public string Image { get; set; }

        [Required(ErrorMessage = "Image Empity")]
        public IFormFile ImageFile
        {
            get => _imageFile;
            set
            {
                if (value != null)
                {
                    if (Image == null)
                    {
                        Image = Guid.NewGuid().ToString("N") + Path.GetExtension(value.FileName);
                    }
                    string locatin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/products/", Image);
                    using FileStream stream = new FileStream(locatin, FileMode.Create);
                    value.CopyTo(stream);
                    stream.Close();
                }
                _imageFile = value;
            }
        }
        //private List<IFormFile> _images;
        //public List<string> ProductImages { get; set; } = new List<string>();
        //public List<IFormFile> Images
        //{
        //    get => _images;
        //    set
        //    {
        //        if (value != null)
        //        foreach (var img in value)
        //        {
        //                if(ProductImages.co)
        //            string img_p = Guid.NewGuid().ToString("N") + Path.GetExtension(img.FileName);
        //        }
                
        //        _images = value;
        //    }
        //}

        public List<Categori> Categoris { get; set; }

    }
}
