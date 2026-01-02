using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthMate.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AddDoctorModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddDoctorModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = new IdentityUser 
            { 
                UserName = Email, 
                Email = Email,
                EmailConfirmed = true
            };
            
            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Doctor"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Doctor"));
                }
                
                await _userManager.AddToRoleAsync(user, "Doctor");
                Message = "Doctor added successfully";
                return Page();
            }

            Message = string.Join(", ", result.Errors.Select(e => e.Description));
            return Page();
        }
    }
}
