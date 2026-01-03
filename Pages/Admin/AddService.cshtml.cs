using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HealthMate.Data;
using HealthMate.Models;
using System.ComponentModel.DataAnnotations;

namespace HealthMate.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class AddServiceModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public AddServiceModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [BindProperty]
        public IFormFile? Image { get; set; }

        public class InputModel
        {
            [Required]
            public string Name { get; set; } = "";

            [Required]
            public string Description { get; set; } = "";

            [Required]
            public decimal Price { get; set; }

            [Required]
            public string Category { get; set; } = "";

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

            var service = new OtherService
            {
                Name = Input.Name,
                Description = Input.Description,
                Price = Input.Price,
                Category = Input.Category,
                IsAvailable = Input.IsAvailable
            };

            _context.OtherServices.Add(service);
            await _context.SaveChangesAsync();

            if (Image != null)
            {
                var path = Path.Combine(_env.WebRootPath, "images", "services", Image.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
            }

            return RedirectToPage("/Admin/Services");
        }
    }
}