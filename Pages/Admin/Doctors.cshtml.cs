using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HealthMate.Data;
using HealthMate.Models;

namespace HealthMate.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DoctorsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DoctorsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Doctor> Doctors { get; set; } = new();

        public async Task OnGetAsync()
        {
            Doctors = await _context.Doctors.OrderBy(d => d.Name).ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}