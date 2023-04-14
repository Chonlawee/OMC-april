using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using OMC.Models;
using OMC.Data;

namespace OMC.Pages
{
    public class IndexModel : PageModel
    {

        private readonly OMCContext _context;


        public IndexModel(OMCContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get; set; } = default!;

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