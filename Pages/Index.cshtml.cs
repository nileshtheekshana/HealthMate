using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HealthMate.Data;
using HealthMate.Models;

namespace HealthMate.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Doctor> Doctors { get; set; } = new();
        public List<string> Specializations { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string SearchName { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string SearchSpecialization { get; set; } = "";

        public async Task OnGetAsync()
        {
            Specializations = await _context.Doctors
                .Where(d => d.IsAvailable)
                .Select(d => d.Specialization)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();

            var query = _context.Doctors.Where(d => d.IsAvailable);

            if (!string.IsNullOrEmpty(SearchSpecialization))
            {
                query = query.Where(d => d.Specialization == SearchSpecialization);
            }

            Doctors = await query.OrderBy(d => d.Name).ToListAsync();
        }
    }
}
