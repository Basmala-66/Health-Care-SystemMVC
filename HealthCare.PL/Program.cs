using Health_Care_System.Data;
using Health_Care_System.Entities;
using Health_Care_System.PL.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder=WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration);
var app=builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var appDb = services.GetRequiredService<AppDbContext>();
        await appDb.Database.MigrateAsync();
        var authDb = services.GetRequiredService<AuthenticationDbContext>();
        await authDb.Database.MigrateAsync();

        if (!await appDb.Doctors.AnyAsync())
        {
            var seedPath = Path.Combine(app.Environment.ContentRootPath, "Doctors.json");
            if (File.Exists(seedPath))
            {
                var json = await File.ReadAllTextAsync(seedPath);
                var doctors = JsonSerializer.Deserialize<List<Doctor>>(json);
                if (doctors is { Count: > 0 })
                {
                    await appDb.Doctors.AddRangeAsync(doctors);
                    await appDb.SaveChangesAsync();
                }
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while preparing the database.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
