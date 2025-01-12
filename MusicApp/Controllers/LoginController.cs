using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Helpers;
using MusicApp.Models;
using System.Linq;

namespace MusicApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly MusicAppDbContext _context;

        public LoginController(MusicAppDbContext context)
        {
            _context = context;
        }

        // Giriş Sayfası
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);

            if (user != null && PasswordHelper.VerifyPassword(password, user.Password)) // Şifre doğrulama
            {
                if (!user.IsActive)
                {
                    ViewBag.Error = "Hesabınız şu anda aktif değil. Lütfen sistem yöneticisi ile iletişime geçin.";
                    return View();
                }

                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        // Kayıt Sayfası
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string email, string password)
        {
            if (_context.Users.Any(u => u.Username == username || u.Email == email))
            {
                ViewBag.Error = "Bu kullanıcı adı veya e-posta zaten alınmış!";
                return View();
            }

            // Şifreyi hashle
            var hashedPassword = PasswordHelper.HashPassword(password);

            // Yeni kullanıcıyı oluştur
            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                Role = "User",
                IsActive = true // Varsayılan olarak aktif
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            ViewBag.Message = "Hesabınız başarıyla oluşturuldu. Giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }

        // Çıkış Yap
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Oturum bilgisini temizle
            return RedirectToAction("Login");
        }
    }
}
