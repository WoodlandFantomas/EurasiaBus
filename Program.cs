using EurasiaBus.Data;
using EurasiaBus.Models;
using EurasiaBus.Services;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

using QuestPDF.Infrastructure;

using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


// ============================================================
// DATABASE
// ============================================================

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySQL(connectionString!);
});


// ============================================================
// IDENTITY
// ============================================================

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;

        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


// ============================================================
// COOKIE SETTINGS
// ============================================================

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});


// ============================================================
// LOCALIZATION
// ============================================================

builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "Resources";
});


// ============================================================
// MVC
// ============================================================

builder.Services
    .AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();


// ============================================================
// RAZOR PAGES
// ============================================================

builder.Services.AddRazorPages();


// ============================================================
// ROUTES EXCEL SERVICE
// ============================================================

builder.Services.AddScoped<RouteExcelService>();


// ============================================================
// ROUTE TEXT SERVICE
// ============================================================

builder.Services.AddScoped<RouteTextService>();


// ============================================================
// HTTP CONTEXT
// ============================================================

builder.Services.AddHttpContextAccessor();


// ============================================================
// QUESTPDF
// ============================================================

// Для текущего проекта используем Community license.
// Если ваш проект использует другой тип лицензии,
// эту строку можно изменить.

QuestPDF.Settings.License = LicenseType.Community;


// ============================================================
// BUILD APPLICATION
// ============================================================

var app = builder.Build();


// ============================================================
// LOCALIZATION SETTINGS
// ============================================================

var supportedCultures = new[]
{
    new CultureInfo("ru"),
    new CultureInfo("uk"),
    new CultureInfo("en")
};

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru"),

    SupportedCultures = supportedCultures,

    SupportedUICultures = supportedCultures
};


// ============================================================
// LANGUAGE COOKIE HAS PRIORITY
// ============================================================

localizationOptions.RequestCultureProviders.Insert(
    0,
    new CookieRequestCultureProvider());


// ============================================================
// REQUEST LOCALIZATION
// ============================================================

app.UseRequestLocalization(localizationOptions);


// ============================================================
// ERROR HANDLING
// ============================================================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}


// ============================================================
// HTTPS
// ============================================================

app.UseHttpsRedirection();


// ============================================================
// STATIC FILES
// ============================================================

app.UseStaticFiles();


// ============================================================
// ROUTING
// ============================================================

app.UseRouting();


// ============================================================
// AUTHENTICATION
// ============================================================

app.UseAuthentication();


// ============================================================
// AUTHORIZATION
// ============================================================

app.UseAuthorization();


// ============================================================
// MVC ROUTE
// ============================================================

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// ============================================================
// RAZOR PAGES
// ============================================================

app.MapRazorPages();


// ============================================================
// DATABASE INITIALIZATION
// ============================================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context =
            services.GetRequiredService<ApplicationDbContext>();

        // Создание/обновление базы данных
        await context.Database.MigrateAsync();

        // Заполнение ролей, администратора и начальных данных
        await DbInitializer.SeedAsync(services);
    }
    catch (Exception ex)
    {
        var logger =
            services.GetRequiredService<ILogger<Program>>();

        logger.LogError(
            ex,
            "Ошибка при инициализации базы данных.");
    }
}


// ============================================================
// START APPLICATION
// ============================================================

app.Run();