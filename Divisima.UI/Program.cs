using Divisima.BL.Repositories;
using Divisima.DAL.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SQLContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CS1")));
builder.Services.AddScoped(typeof(IRepository<>), typeof(SQLRepository<>));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(opt =>
{
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    opt.LoginPath = "/admin/login"; // yetkisi olmadan giri� yapmaya �al���rsa
    opt.LogoutPath = "/admin/logout"; // oturum s�resi dolarsa veya kullan�c� ��k�� yaparsa
});
var app = builder.Build();
if (!app.Environment.IsDevelopment()) app.UseStatusCodePagesWithRedirects("/hata/{0}");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // kimlik do�rulama
app.UseAuthorization(); // kimlik yetkilendirme
app.MapControllerRoute(name: "areas", pattern: "{area=exists}/{controller=home}/{action=index}/{id?}");
app.MapControllerRoute(name: "default", pattern: "{controller=home}/{action=index}/{id?}");
app.Run();
