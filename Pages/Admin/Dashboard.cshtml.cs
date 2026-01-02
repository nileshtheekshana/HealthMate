using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HealthMate.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public DashboardModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public int TotalUsers { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }

        public async Task OnGetAsync()
        {
            TotalUsers = await _userManager.Users.CountAsync();
            
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            TotalDoctors = doctors.Count;
            
            var patients = await _userManager.GetUsersInRoleAsync("Patient");
            TotalPatients = patients.Count;
        }
    }
}
