using BestStoreMVC.Data;
using BestStoreMVC.Entity;
using Microsoft.AspNetCore.Identity;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using sib_api_v3_sdk.Client;
using BestStoreMVC.Implementation.Services;
using AspNetCoreHero.ToastNotification;
using BestStoreMVC.Services;
using BestStoreMVC.Implementation.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IBookService, BookService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Configure Notyf for notifications
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 3;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
	options =>
	{
		options.Password.RequiredLength = 6;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequireUppercase = false;
		options.Password.RequireLowercase = false;
	})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

Configuration.Default.ApiKey.Add("api-key", builder.Configuration["BrevoSettings:ApiKey"]);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for Bookion scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



// create the roles and the first admin user if not available yet
using (var scope = app.Services.CreateScope())
{
	var userManager = scope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>))
		as UserManager<ApplicationUser>;
	var roleManager = scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>))
		as RoleManager<IdentityRole>;

	await DatabaseInitializer.SeedDataAsync(userManager, roleManager);
}


app.Run();
