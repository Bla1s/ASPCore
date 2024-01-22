using ASPCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
builder.Services.AddDbContext<FilmDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:FilmDbConnection"]));
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(opts =>
{
	opts.Password.RequiredLength = 8;
	opts.Password.RequireLowercase = true;
    opts.User.RequireUniqueEmail = true;
});
builder.Services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Account/Login");
builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.Name = ".AspNetCore.Identity.Application";
	options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
	options.SlidingExpiration = true;
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

app.Run();
