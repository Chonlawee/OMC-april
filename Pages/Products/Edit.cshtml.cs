using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OMC.Data;
using OMC.Models;

namespace OMC.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly OMC.Data.OMCContext _context;

        public EditModel(OMC.Data.OMCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product =  await _context.Product.FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            Product = product;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
           

            if (!ModelState.IsValid)
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

            _context.Attach(Product).State = EntityState.Modified;

            try
            {
                Product.Modified = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.ProductID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductID == id)).GetValueOrDefault();
        }
    }
}
