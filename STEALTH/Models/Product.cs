using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace STEALTH.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public List<CategoryProductBrig> CategoryProducts { get; set; }
        public List<BasketProduct> BasketProducts { get; set; }

        private IFormFile _imageFile;
        public string Image { get; set; }
        [Required(ErrorMessage = "Image Empity")]
        [NotMapped]
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
                    FileStream stream = new FileStream(locatin, FileMode.Create);
                    value.CopyTo(stream);
                }
                _imageFile = value;
            }
        }
    }


    //public class Item
    //{

    //    private IFormFile _imageFile;
    //    public int Id { get; set; }
    //    [Required(ErrorMessage = "Title Empity")]
    //    public string Title { get; set; }
    //    [Required(ErrorMessage = "Description Empity")]
    //    public string Description { get; set; }
    //    public string Image { get; set; }
    //    [Required(ErrorMessage = "Image Empity")]
    //    [NotMapped]
    //    public IFormFile ImageFile
    //    {
    //        get => _imageFile;
    //        set
    //        {



    //            if (value != null)
    //            {
    //                if (Image == null)
    //                {
    //                    Image = Guid.NewGuid().ToString("N") + Path.GetExtension(value.FileName);
    //                }
    //                string locatin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/products/", Image);
    //                FileStream stream = new FileStream(locatin, FileMode.Create);
    //                value.CopyTo(stream);
    //            }
    //            _imageFile = value;
    //        }
    //    }
    //    [Required(ErrorMessage = "Price Empity")]
    //    public double Price { get; set; }
    //    [Required(ErrorMessage = "Color Empity")]
    //    public string Color { get; set; }
    //    [Required(ErrorMessage = "ProductCategory Empity")]
    //    public string ProductCategory { get; set; }
    //    public double Sale { get; set; }
    //}
}
