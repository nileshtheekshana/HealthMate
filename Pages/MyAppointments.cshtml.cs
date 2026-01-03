using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HealthMate.Data;
using HealthMate.Models;
using System.Security.Claims;

namespace HealthMate.Pages
{
    [Authorize(Roles = "Patient")]
    public class MyAppointmentsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MyAppointmentsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Appointment> Appointments { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                Appointments = await _context.Appointments
                    .Include(a => a.Doctor)
                    .Where(a => a.PatientId == userId)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToListAsync();
            }
        }

        public async Task<IActionResult> OnPostCancelAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id && a.PatientId == userId);

            if (appointment != null)
            {
                appointment.Status = "Cancelled";
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
