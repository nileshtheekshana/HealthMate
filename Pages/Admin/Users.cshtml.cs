using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HealthMate.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class UsersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public List<UserWithRole> Users { get; set; } = new();

        public class UserWithRole
        {
            public string Id { get; set; } = "";
            public string Email { get; set; } = "";
            public bool EmailConfirmed { get; set; }
            public string Role { get; set; } = "";
        }

        public async Task OnGetAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                Users.Add(new UserWithRole
                {
                    Id = user.Id,
                    Email = user.Email ?? "",
                    EmailConfirmed = user.EmailConfirmed,
                    Role = roles.FirstOrDefault() ?? "No Role"
                });
            }
        }
    }
}