using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
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
            var user = _context.Users.SingleOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin"); // Admin paneline yönlendir
                }
                else
                {
                    return RedirectToAction("Index", "Song"); // Kullanıcı ana sayfasına yönlendir
                }
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
        public IActionResult Register(string username, string email, string password, string role = "User")
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Bu kullanıcı adı zaten alınmış!";
                return View();
            }

            var user = new User
            {
                Username = username,
                Email = email,
                Password = password,
                Role = role
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login", "Login");
        }

        // Çıkış Yap
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Oturum bilgisini temizle
            return RedirectToAction("Login", "Login");
        }
    }
}
