using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OMC.Data;
using OMC.Models;

namespace OMC.Pages.Recipes
{
    public class IndexModel : PageModel
    {
        private readonly OMC.Data.OMCContext _context;

        public IndexModel(OMC.Data.OMCContext context)
        {
            _context = context;
        }

        public IList<Recipe> Recipe { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Recipe != null)
            {
                Recipe = await _context.Recipe
                .Include(r => r.Product)
                .Where(r=>r.Deleted ==null)
                .ToListAsync();
            }
        }
    }
}
