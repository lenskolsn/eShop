using eShop.Database;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using eShop.WebConfigs;
using eShop.Areas.Admin.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Kết nối db
builder.Services.AddDbContext<AppDbContext>(opts =>
{
    var connectionString = builder.Configuration.GetConnectionString("DB");
    opts.UseSqlServer(connectionString);
});

// Cấu hình mapper
var mapperConfig = new MapperConfiguration(opt =>
{
    opt.AddProfile<AutoMapperProfile>();
});

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// cấu hình đăng nhập bằng cookies
builder.Services.AddAuthentication("Cookies").AddCookie(opt =>
{
    opt.LoginPath = "/login";
    opt.ExpireTimeSpan = TimeSpan.FromHours(8); // hết hạn sau 8h   
    opt.Cookie.HttpOnly = true; // tầng bảo mật

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

app.UseAuthentication(); // phải nằm trên UseAuthorization()
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "AdminArea",
    areaName: "Admin",
    pattern: "/Admin/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

PathHelper.SetWebRootPath(app.Environment.WebRootPath);

app.Run();
