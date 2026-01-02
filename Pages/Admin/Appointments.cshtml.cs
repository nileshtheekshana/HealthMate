using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HealthMate.Data;
using HealthMate.Models;

namespace HealthMate.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AppointmentsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Appointment> Appointments { get; set; } = new();

        public async Task OnGetAsync()
        {
            Appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostConfirmAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                appointment.Status = "Confirmed";
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}