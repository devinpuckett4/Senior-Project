using ChurchOutreachInventory.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DefaultConnection is missing. Configure it in User Secrets.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/dev/database", async (ApplicationDbContext database, CancellationToken cancellationToken) =>
    {
        var connected = await database.Database.CanConnectAsync(cancellationToken);

        return connected
            ? Results.Text("Database connection successful.")
            : Results.Problem(
                title: "Database connection failed.",
                detail: "Check the MySQL service and your local connection settings.",
                statusCode: StatusCodes.Status503ServiceUnavailable);
    });
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();