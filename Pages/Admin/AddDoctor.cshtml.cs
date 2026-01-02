using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HealthMate.Data;
using HealthMate.Models;

namespace HealthMate.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AddDoctorModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AddDoctorModel(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        [BindProperty]
        public string Name { get; set; } = "";

        [BindProperty]
        public string Specialization { get; set; } = "";

        [BindProperty]
        public string Qualification { get; set; } = "";

        [BindProperty]
        public string Experience { get; set; } = "";

        [BindProperty]
        public string ContactNumber { get; set; } = "";

        public string Message { get; set; } = "";

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = new IdentityUser { UserName = Email, Email = Email };
            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Doctor");

                var doctor = new Doctor
                {
                    UserId = user.Id,
                    Name = Name,
                    Specialization = Specialization,
                    Qualification = Qualification,
                    Experience = Experience,
                    ContactNumber = ContactNumber,
                    IsAvailable = true
                };

                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();

                Message = "Doctor added successfully!";
                return Page();
            }

            Message = "Error: " + string.Join(", ", result.Errors.Select(e => e.Description));
            return Page();
        }
    }
}
