using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Models;
using System.Linq;

namespace MusicApp.Controllers
{
    public class UserController : Controller
    {
        private readonly MusicAppDbContext _context;

        public UserController(MusicAppDbContext context)
        {
            _context = context;
        }

        // Profil Sayfası
        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Index", "Login");

            return View(user);
        }

        // Profil Güncelleme
        [HttpPost]
        public IActionResult UpdateProfile(int id, string username, string email)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.Username = username;
                user.Email = email;
                _context.SaveChanges();
                HttpContext.Session.SetString("Username", username); // Oturumdaki kullanıcı adını güncelle
            }

            return RedirectToAction("Profile");
        }

        // Şifre Güncelleme
        [HttpPost]
        public IActionResult UpdatePassword(int id, string currentPassword, string newPassword)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == id);
            if (user != null && user.Password == currentPassword)
            {
                user.Password = newPassword;
                _context.SaveChanges();
                ViewBag.Message = "Şifre başarıyla güncellendi.";
            }
            else
            {
                ViewBag.Error = "Mevcut şifre hatalı.";
            }

            return View("Profile", user);
        }
    }
}
