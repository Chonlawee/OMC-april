using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OMC.Data;
using OMC.Models;

namespace OMC.Pages
{
    public class HistoryModel : PageModel
    {
        private readonly OMCContext _context;
        public HistoryModel(OMCContext context)
        {
            _context = context;
        }
        public IList<Order> Order { get; set; } = default!;
        public IList<Product> Product { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Order != null)
            {
                Order = await _context.Order.ToListAsync();

            }
            if (_context.Product != null)
            {
                Product = await _context.Product.ToListAsync();

            }
        }
    }
}
