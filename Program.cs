using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HealthMate.Data;
using HealthMate.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    
    if (!context.Doctors.Any())
    {
        var user1 = new IdentityUser { UserName = "doctor1@test.com", Email = "doctor1@test.com", EmailConfirmed = true };
        await userManager.CreateAsync(user1, "Doctor@123");
        await userManager.AddToRoleAsync(user1, "Doctor");
        
        context.Doctors.Add(new Doctor
        {
            UserId = user1.Id,
            Name = "John Smith",
            Specialization = "Cardiology",
            Qualification = "MBBS, MD",
            Experience = "10",
            ContactNumber = "123-456-7890",
            IsAvailable = true
        });

        var user2 = new IdentityUser { UserName = "doctor2@test.com", Email = "doctor2@test.com", EmailConfirmed = true };
        await userManager.CreateAsync(user2, "Doctor@123");
        await userManager.AddToRoleAsync(user2, "Doctor");
        
        context.Doctors.Add(new Doctor
        {
            UserId = user2.Id,
            Name = "Sarah Johnson",
            Specialization = "Dermatology",
            Qualification = "MBBS, MD",
            Experience = "8",
            ContactNumber = "123-456-7891",
            IsAvailable = true
        });

        var user3 = new IdentityUser { UserName = "doctor3@test.com", Email = "doctor3@test.com", EmailConfirmed = true };
        await userManager.CreateAsync(user3, "Doctor@123");
        await userManager.AddToRoleAsync(user3, "Doctor");
        
        context.Doctors.Add(new Doctor
        {
            UserId = user3.Id,
            Name = "Mike Brown",
            Specialization = "Pediatrics",
            Qualification = "MBBS, MD",
            Experience = "12",
            ContactNumber = "123-456-7892",
            IsAvailable = true
        });

        await context.SaveChangesAsync();
    }
}

app.Run();
