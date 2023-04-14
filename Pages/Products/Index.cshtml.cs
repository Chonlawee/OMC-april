using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OMC.Data;
using OMC.Models;

namespace OMC.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly OMC.Data.OMCContext _context;

        public IndexModel(OMC.Data.OMCContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Product != null)
            {
                Product = await _context.Product
                    .Where(i => i.Deleted == null)
                    .ToListAsync();
            }
        }
    }
}
