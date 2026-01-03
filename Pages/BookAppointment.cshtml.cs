using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HealthMate.Data;
using HealthMate.Models;
using System.ComponentModel.DataAnnotations;

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

        public Models.Doctor? Doctor { get; set; }
        public string? ErrorMessage { get; set; }
        public List<string> TimeSlots { get; set; } = new() { "09:00 AM", "10:00 AM", "11:00 AM", "02:00 PM", "03:00 PM", "04:00 PM" };

        [BindProperty]
        public DateTime AppointmentDate { get; set; }

        [BindProperty]
        public string TimeSlot { get; set; } = "";

        [BindProperty]
        public string Reason { get; set; } = "";

        [BindProperty]
        public int DoctorId { get; set; }

        public async Task<IActionResult> OnGetAsync(int doctorId)
        {
            Doctor = await _context.Doctors.FindAsync(doctorId);
            if (Doctor == null)
            {
                return NotFound();
            }
            DoctorId = doctorId;
            AppointmentDate = DateTime.Today.AddDays(1);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Doctor = await _context.Doctors.FindAsync(DoctorId);
            if (Doctor == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(TimeSlot) || string.IsNullOrEmpty(Reason))
            {
                ErrorMessage = "Please fill all fields";
                return Page();
            }

            var appointment = new Appointment
            {
                PatientId = User.Identity?.Name ?? "",
                DoctorId = DoctorId,
                AppointmentDate = AppointmentDate,
                TimeSlot = TimeSlot,
                Reason = Reason,
                Status = "Pending",
                CreatedAt = DateTime.Now
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToPage("/MyAppointments");
        }
    }
}