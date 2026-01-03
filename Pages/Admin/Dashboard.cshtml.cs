using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HealthMate.Data;
using HealthMate.Models;

namespace HealthMate.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DashboardModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public int TotalUsers { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public int TotalAppointments { get; set; }
        public int PendingAppointments { get; set; }

        public async Task OnGetAsync()
        {
            TotalUsers = await _userManager.Users.CountAsync();
            TotalDoctors = await _context.Doctors.CountAsync();
            
            var patientRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Patient");
            if (patientRole != null)
            {
                TotalPatients = await _context.UserRoles.CountAsync(ur => ur.RoleId == patientRole.Id);
            }

            TotalAppointments = await _context.Appointments.CountAsync();
            PendingAppointments = await _context.Appointments.CountAsync(a => a.Status == "Pending");
        }
    }
}
