using Microsoft.EntityFrameworkCore;
using MusicApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Diðer servis eklemeleriniz...
builder.Services.AddHttpContextAccessor();


// Veritabaný baðlantýsýný yapýlandýr
builder.Services.AddDbContext<MusicAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session servislerini ekle
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
    options.Cookie.HttpOnly = true; // Güvenlik için yalnýzca sunucu eriþimi
    options.Cookie.IsEssential = true; // Çerezin zorunlu olduðunu belirt
});

// MVC servislerini ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS yönlendirme ve statik dosyalar
app.UseHttpsRedirection();
app.UseStaticFiles();

// Middleware sýralamasý
app.UseRouting();

// Session middleware
app.UseSession();

// Authorization (Yetkilendirme için gerekli)
app.UseAuthorization();

// Varsayýlan rotayý ayarla
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=login}/{id?}");

// Uygulamayý çalýþtýr
app.Run();
