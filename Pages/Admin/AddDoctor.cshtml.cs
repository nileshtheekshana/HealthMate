using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using HealthMate.Data;
using HealthMate.Models;
using System.ComponentModel.DataAnnotations;

namespace HealthMate.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AddDoctorModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddDoctorModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required]
            public string Name { get; set; } = "";

            [Required]
            public string Specialization { get; set; } = "";

            [Required]
            public string Qualification { get; set; } = "";

            [Required]
            public int Experience { get; set; }

            [Required]
            public string ContactNumber { get; set; } = "";

            [Required]
            [EmailAddress]
            public string Email { get; set; } = "";

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = "";

            public bool IsAvailable { get; set; } = true;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = new IdentityUser { UserName = Input.Email, Email = Input.Email, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Doctor"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Doctor"));
                }
                await _userManager.AddToRoleAsync(user, "Doctor");

                var doc = new Models.Doctor
                {
                    Name = Input.Name,
                    Specialization = Input.Specialization,
                    Qualification = Input.Qualification,
                    Experience = Input.Experience,
                    ContactNumber = Input.ContactNumber,
                    Email = Input.Email,
                    IsAvailable = Input.IsAvailable
                };

                _context.Doctors.Add(doc);
                await _context.SaveChangesAsync();

                return RedirectToPage("/Admin/Doctors");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
