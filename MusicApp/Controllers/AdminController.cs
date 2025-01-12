using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MusicApp.Data;
using MusicApp.Models;
using System.IO;
using System.Linq;

namespace MusicApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly MusicAppDbContext _context;

        public AdminController(MusicAppDbContext context)
        {
            _context = context;
        }

        // Action çalıştırılmadan önce admin kontrolü
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                context.Result = RedirectToAction("Login", "Login");
            }

            base.OnActionExecuting(context);
        }

        // Admin Paneli Ana Sayfası
        public IActionResult Index()
        {
            var songs = _context.Songs.ToList();
            return View(songs);
        }

        // Yeni Şarkı Ekleme Sayfası
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Song song, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/Sarkilar", file.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                song.FilePath = $"Sarkilar/{file.FileName}";
            }

            _context.Songs.Add(song);
            _context.SaveChanges();
            TempData["Message"] = "Şarkı başarıyla eklendi.";
            return RedirectToAction("Index");
        }

        // Şarkı Düzenleme Sayfası
        public IActionResult Edit(int id)
        {
            var song = _context.Songs.Find(id);
            if (song == null)
                return NotFound();

            return View(song);
        }

        [HttpPost]
        public IActionResult Edit(Song song, IFormFile file)
        {
            var existingSong = _context.Songs.Find(song.Id);
            if (existingSong != null)
            {
                existingSong.Title = song.Title;
                existingSong.Artist = song.Artist;
                existingSong.Album = song.Album;
                existingSong.Genre = song.Genre;

                if (file != null && file.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/Sarkilar", file.FileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(stream);
                    existingSong.FilePath = $"Sarkilar/{file.FileName}";
                }

                _context.SaveChanges();
                TempData["Message"] = "Şarkı başarıyla güncellendi.";
            }

            return RedirectToAction("Index");
        }

        // Şarkı Silme
        public IActionResult Delete(int id)
        {
            var song = _context.Songs.Find(id);
            if (song == null)
                return NotFound();

            _context.Songs.Remove(song);
            _context.SaveChanges();
            TempData["Message"] = "Şarkı başarıyla silindi.";
            return RedirectToAction("Index");
        }

        // Kullanıcıları Listele
        public IActionResult ManageUsers()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // Kullanıcı Aktiflik Durumunu Değiştir
        [HttpPost]
        public IActionResult ToggleUserStatus(int userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.IsActive = !user.IsActive;
                _context.SaveChanges();
                TempData["Message"] = $"Kullanıcı {user.Username} {(user.IsActive ? "aktif" : "pasif")} duruma getirildi.";
            }

            return RedirectToAction("ManageUsers");
        }

        // Profil Sayfası
        public IActionResult Profile()
        {
            var username = HttpContext.Session.GetString("Username");
            var adminUser = _context.Users.SingleOrDefault(u => u.Username == username);

            if (adminUser == null)
                return RedirectToAction("Login", "Login");

            return View(adminUser);
        }
    }
}
