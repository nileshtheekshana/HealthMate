using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HealthMate.Data;
using HealthMate.Models;

namespace HealthMate.Pages.Doctor
{
    [Authorize(Roles = "Doctor")]
    public class PortalModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PortalModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Models.Doctor? DoctorInfo { get; set; }
        public List<Appointment> Appointments { get; set; } = new();
        public int TodayCount { get; set; }
        public int PendingCount { get; set; }
        public int CompletedCount { get; set; }
        public string? DebugEmail { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            DebugEmail = User.Identity?.Name;
            DoctorInfo = await _context.Doctors.FirstOrDefaultAsync(d => d.Email == DebugEmail);

            if (DoctorInfo == null)
            {
                return Page();
            }

            Appointments = await _context.Appointments
                .Where(a => a.DoctorId == DoctorInfo.Id)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            TodayCount = Appointments.Count(a => a.AppointmentDate.Date == DateTime.Today);
            PendingCount = Appointments.Count(a => a.Status == "Pending");
            CompletedCount = Appointments.Count(a => a.Status == "Completed");

            return Page();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync()
        {
            var email = User.Identity?.Name;
            var doc = await _context.Doctors.FirstOrDefaultAsync(d => d.Email == email);

            if (doc != null)
            {
                doc.IsAvailable = !doc.IsAvailable;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}