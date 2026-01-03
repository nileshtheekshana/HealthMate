using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HealthMate.Data;
using HealthMate.Models;

namespace HealthMate.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ServicesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ServicesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<OtherService> Services { get; set; } = new();

        public async Task OnGetAsync()
        {
            Services = await _context.OtherServices.OrderBy(s => s.Name).ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var service = await _context.OtherServices.FindAsync(id);
            if (service != null)
            {
                _context.OtherServices.Remove(service);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}