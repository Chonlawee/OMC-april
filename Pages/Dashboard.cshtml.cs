using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OMC.Data;
using OMC.Models;

namespace OMC.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly OMCContext _context;
        public DashboardModel(OMCContext context)
        {
            _context = context;
        }
        public IList<Order> Order { get; set; } = default!;

        public async Task OnGetAsync(int orderId)
        {
            if (_context.Order != null)
            {
               Order = await _context.Order.ToListAsync();
            }
        }
        public async Task<IActionResult> OnPostMarkAsDoneAsync(int id)
        {
            // Find the order with the specified ID
            var order = await _context.Order.FirstOrDefaultAsync(o => o.OrderID == id);

            if (order == null)
            {
                // Return a 404 error if the order is not found
                return NotFound();
            }

            // Update the order status to "Done"
            order.Status = "Done";

            // Update the queue positions of any remaining orders in the queue
            foreach (var remainingOrder in await _context.Order.Where(o => o.Status != "Done").OrderBy(o => o.QueuePosition).ToListAsync())
            {
                remainingOrder.QueuePosition--;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage();

        }
    }
}

