using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OMC.Data;
using OMC.Models;
namespace OMC.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly OMC.Data.OMCContext _context;

        public CreateModel(OMC.Data.OMCContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
           
            if (!ModelState.IsValid || _context.Product == null || Product == null)
            {
                return Page();
            }

            byte[] bytes = null;


            if (Product.ImgFile != null)
            {
                using (Stream fs = Product.ImgFile
                    .OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        bytes = br.ReadBytes((Int32)fs.Length);
                    }
                    Product.ProductImage = Convert.ToBase64String(bytes, 0, bytes.Length);
                }
            }

            if (Product.ProductType == "กาแฟ")
            {
                Product.ProductTypeNumber = 1;
            }
            
            else if (Product.ProductType == "ชาไทย")
            {
                Product.ProductTypeNumber = 2;
            }

            else if (Product.ProductType == "ชาเขียว")
            {
                Product.ProductTypeNumber = 3;
            }

            Product.Created = DateTime.Now;
            Product.Modified = DateTime.Now;
            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
