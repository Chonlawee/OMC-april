using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OMC.Data;
using OMC.Models;

namespace OMC.Pages.Recipes
{
    public class CreateModel : PageModel
    {
        private readonly OMCContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(OMCContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
        ViewData["ProductID"] = new SelectList(_context.Product, "ProductID", "ProductID");
            return Page();
        }

        [BindProperty]
        public Recipe Recipe { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var product = await _context.Product.FindAsync(Recipe.ProductID);
            if (product == null)
            {
                return NotFound();
            }

            Recipe.Product = product;


            if (!ModelState.IsValid )
            {
                _logger.LogInformation($"Recipe: {Recipe.RecipeName}, {Recipe.ProductID}, {Recipe.Syrup}, {Recipe.Milk}, {Recipe.Water}, {Recipe.Deleted}");
                return Page();
            }
            
            Recipe.Created = DateTime.Now;
            Recipe.Modified = DateTime.Now;
            _context.Recipe.Add(Recipe);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
