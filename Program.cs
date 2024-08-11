using ELibrary.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Vite.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
	builder.Services.AddDbContext<ELibraryContext>(options =>
		options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
}
else
{
	var connectionString = builder.Configuration.GetConnectionString("MySql");
	builder.Services.AddDbContext<ELibraryContext>(options =>
		options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddViteServices();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/Account/Login";
		options.LogoutPath = "/Account/Logout";
		options.AccessDeniedPath = "/Account/AccessDenied";
		options.ReturnUrlParameter = "ReturnUrl";
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

if (app.Environment.IsDevelopment())
{
	// WebSockets support is required for HMR (hot module reload).
	// Uncomment the following line if your pipeline doesn't contain it.
	app.UseWebSockets();
	// Enable all required features to use the Vite Development Server.
	// Pass true if you want to use the integrated middleware.
	app.UseViteDevelopmentServer(true);
}

app.Run();
