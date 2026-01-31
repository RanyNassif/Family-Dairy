using DairyFamilyManager.Data;
using DairyFamilyManager.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization();
builder.Services.AddRazorPages().AddViewLocalization().AddDataAnnotationsLocalization();

string? provider = builder.Configuration["DatabaseProvider"];
string sqlServerConn = builder.Configuration.GetConnectionString("SqlServer") ?? "";
string postgresConn = builder.Configuration.GetConnectionString("Postgres") ?? "";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (string.Equals(provider, "Postgres", StringComparison.OrdinalIgnoreCase))
    {
        options.UseNpgsql(postgresConn);
        return;
    }

    options.UseSqlServer(sqlServerConn);
});

builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<SeedService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;

    options.Cookie.Name = "DairyFamily.Auth";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
    options.AddPolicy("DataEntryOrAdmin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin", "DataEntry"));
});

WebApplication app = builder.Build();

CultureInfo[] supportedCultures = new[]
{
    new CultureInfo("en-US"),
    new CultureInfo("ar-LB")
};

RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture("en-US")
    .AddSupportedCultures(supportedCultures.Select(c => c.Name).ToArray())
    .AddSupportedUICultures(supportedCultures.Select(c => c.Name).ToArray());

localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

using (IServiceScope scope = app.Services.CreateScope())
{
    SeedService seed = scope.ServiceProvider.GetRequiredService<SeedService>();
    await seed.SeedUsersAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseRequestLocalization(localizationOptions);

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
