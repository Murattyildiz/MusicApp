using Microsoft.EntityFrameworkCore;
using MusicApp.Data;
using MusicApp.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor(); // HttpContext'e eri�im i�in gerekli

// Veritaban� ba�lant�s�n� yap�land�r
builder.Services.AddDbContext<MusicAppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session servislerini ekle
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum s�resi
    options.Cookie.HttpOnly = true; // G�venlik i�in yaln�zca sunucu eri�imi
    options.Cookie.IsEssential = true; // �erezin zorunlu oldu�unu belirt
});

builder.Services.AddHttpClient();

// MVC servislerini ekle
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS y�nlendirme ve statik dosyalar
app.UseHttpsRedirection();
app.UseStaticFiles();

// Middleware s�ralamas�
app.UseRouting();
app.UseSession(); // Session middleware
app.UseAuthorization(); // Yetkilendirme

// Varsay�lan rotay� ayarla
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

//// Veri tohumlama (admin kullan�c�s�n� eklemek i�in)
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<MusicAppDbContext>();

//    if (!context.Users.Any(u => u.Username == "admin"))
//    {
//        var adminUser = new MusicApp.Models.User
//        {
//            Username = "admin",
//            Email = "muratyildiz42953@gmail.com", // E-posta adresi d�zeltildi
//            Password = PasswordHelper.HashPassword("admin123"), // �ifreyi hashleyerek kaydediyoruz
//            Role = "Admin",
//            IsActive = true
//        };

//        context.Users.Add(adminUser);
//        context.SaveChanges();
//        Console.WriteLine("Admin kullan�c�s� ba�ar�yla olu�turuldu.");
//    }
//    else
//    {
//        Console.WriteLine("Admin kullan�c�s� zaten mevcut.");
//    }
//}


app.Run();
