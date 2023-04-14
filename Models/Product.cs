using Microsoft.AspNetCore.Identity;
using NuGet.Packaging.Signing;
using OMC.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace OMC.Models
{

    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "Coffee";
        public int ProductPrice { get; set; }
        public string ProductImage { get; set; } = "Don't have image";

        public string ProductType { get; set; } = "Null";
        public int ProductTypeNumber { get; set; }
        [NotMapped]
        public IFormFile? ImgFile { get; set; }
        public DateTime Created { get; set; } // new property to store creation timestamp
        public DateTime Modified { get; set; }
        public DateTime? Deleted { get; set; }

    }
     
}

