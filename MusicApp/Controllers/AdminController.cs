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
        public IActionResult Create(Song song, IFormFile file, IFormFile coverImage)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/Sarkilar", file.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                song.FilePath = $"Sarkilar/{file.FileName}";
            }

            if (coverImage != null && coverImage.Length > 0)
            {
                var coverPath = Path.Combine("wwwroot/Images/Covers", coverImage.FileName);
                using var stream = new FileStream(coverPath, FileMode.Create);
                coverImage.CopyTo(stream);
                song.CoverImagePath = $"Images/Covers/{coverImage.FileName}";
            }

            _context.Songs.Add(song);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Düzenleme Sayfasını Getir (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var song = _context.Songs.Find(id);
            if (song == null)
            {
                return NotFound(); // Şarkı bulunamazsa 404 döndür
            }

            return View(song); // Şarkıyı düzenleme formuna gönder
        }

        // Düzenleme İşlemini Kaydet (POST)
        [HttpPost]
        public IActionResult Edit(Song song, IFormFile file, IFormFile coverImage)
        {
            var existingSong = _context.Songs.Find(song.Id);
            if (existingSong != null)
            {
                existingSong.Title = song.Title;
                existingSong.Artist = song.Artist;
                existingSong.Album = song.Album;
                existingSong.Genre = song.Genre;
                existingSong.Lyrics = song.Lyrics;

                // Şarkı dosyasını güncelle
                if (file != null && file.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/Sarkilar", file.FileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(stream);
                    existingSong.FilePath = $"Sarkilar/{file.FileName}";
                }

                // Kapak fotoğrafını güncelle
                if (coverImage != null && coverImage.Length > 0)
                {
                    var coverPath = Path.Combine("wwwroot/Images/Covers", coverImage.FileName);
                    using var stream = new FileStream(coverPath, FileMode.Create);
                    coverImage.CopyTo(stream);
                    existingSong.CoverImagePath = $"Images/Covers/{coverImage.FileName}";
                }

                _context.SaveChanges(); // Değişiklikleri kaydet
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
