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
    public class BookAppointmentModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BookAppointmentModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Doctor Doctor { get; set; } = new();

        [BindProperty]
        public DateTime AppointmentDate { get; set; } = DateTime.Today.AddDays(1);

        [BindProperty]
        public string TimeSlot { get; set; } = "";

        [BindProperty]
        public string Reason { get; set; } = "";

        public string ErrorMessage { get; set; } = "";

        public List<string> TimeSlots { get; set; } = new()
        {
            "09:00 AM", "10:00 AM", "11:00 AM", "12:00 PM",
            "02:00 PM", "03:00 PM", "04:00 PM", "05:00 PM"
        };

        public async Task<IActionResult> OnGetAsync(int doctorId)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null)
            {
                return NotFound();
            }

            Doctor = doctor;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int doctorId)
        {
            var doctor = await _context.Doctors.FindAsync(doctorId);
            if (doctor == null)
            {
                return NotFound();
            }

            Doctor = doctor;

            if (AppointmentDate < DateTime.Today)
            {
                ErrorMessage = "Cannot book past dates";
                return Page();
            }

            if (string.IsNullOrEmpty(TimeSlot))
            {
                ErrorMessage = "Select a time slot";
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var appointment = new Appointment
            {
                PatientId = userId ?? "",
                DoctorId = doctorId,
                AppointmentDate = AppointmentDate,
                TimeSlot = TimeSlot,
                Reason = Reason,
                Status = "Pending"
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToPage("/MyAppointments");
        }
    }
}