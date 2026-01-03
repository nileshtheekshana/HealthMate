using HealthMate.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthMate.Data
{
    public class DbSeeder
    {
        public static void SeedServices(ApplicationDbContext context)
        {
            if (!context.OtherServices.Any())
            {
                context.OtherServices.AddRange(
                    new OtherService { Name = "Blood Test", Description = "Complete blood count and analysis", Price = 50, Category = "Laboratory", IsAvailable = true },
                    new OtherService { Name = "PCR Test", Description = "COVID-19 PCR testing", Price = 75, Category = "Laboratory", IsAvailable = true },
                    new OtherService { Name = "Eye Check", Description = "Comprehensive eye examination", Price = 60, Category = "Vision", IsAvailable = true },
                    new OtherService { Name = "X-Ray", Description = "Digital X-ray imaging", Price = 80, Category = "Radiology", IsAvailable = true },
                    new OtherService { Name = "Ultrasound", Description = "Ultrasound scanning", Price = 100, Category = "Radiology", IsAvailable = true },
                    new OtherService { Name = "ECG Test", Description = "Electrocardiogram test", Price = 55, Category = "Cardiology", IsAvailable = true }
                );
                context.SaveChanges();
            }
        }
    }
}