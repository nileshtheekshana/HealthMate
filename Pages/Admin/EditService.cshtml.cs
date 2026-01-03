using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HealthMate.Data;
using HealthMate.Models;
using System.ComponentModel.DataAnnotations;

namespace HealthMate.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditServiceModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EditServiceModel(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        [BindProperty]
        public IFormFile? Image { get; set; }

        public string? CurrentImage { get; set; }

        public class InputModel
        {
            public int Id { get; set; }

            [Required]
            public string Name { get; set; } = "";

            [Required]
            public string Description { get; set; } = "";

            [Required]
            public decimal Price { get; set; }

            [Required]
            public string Category { get; set; } = "";

            public bool IsAvailable { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var service = await _context.OtherServices.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                Category = service.Category,
                IsAvailable = service.IsAvailable
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var service = await _context.OtherServices.FindAsync(Input.Id);
            if (service == null)
            {
                return NotFound();
            }

            service.Name = Input.Name;
            service.Description = Input.Description;
            service.Price = Input.Price;
            service.Category = Input.Category;
            service.IsAvailable = Input.IsAvailable;

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