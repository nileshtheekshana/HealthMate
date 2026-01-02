using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HealthMate.Pages
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public LoginModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null || user.UserName == null)
            {
                ErrorMessage = "Invalid login attempt";
                return Page();
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, Password, false, false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }

            ErrorMessage = "Invalid login attempt";
            return Page();
        }
    }
}