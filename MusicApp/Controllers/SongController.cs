using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Models;
using System.Linq;

namespace MusicApp.Controllers
{
    public class SongController : Controller
    {
        private readonly MusicAppDbContext _context;

        public SongController(MusicAppDbContext context)
        {
            _context = context;
        }

        // Şarkıların Listelendiği Sayfa
        public IActionResult Index(string searchQuery)
        {
            // Oturum kontrolü
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Login");
            }

            // Kullanıcı kontrolü
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Login");

            // Arama işlemi
            var songs = string.IsNullOrEmpty(searchQuery)
                ? _context.Songs.ToList()  // Arama yapılmadığında tüm şarkıları getir
                : _context.Songs
                    .Where(s => s.Title.Contains(searchQuery) || s.Artist.Contains(searchQuery) || s.Album.Contains(searchQuery))  // Arama yapılırken şarkı başlıkları, sanatçılar ve albümler arasında arama yapılır
                    .ToList();

            // Kullanıcıya ait çalma listelerini getir
            var playlists = _context.Playlists.Where(p => p.UserId == user.Id).ToList();

            // Çalma listelerini ViewBag ile gönder
            ViewBag.Playlists = playlists;

            return View(songs);
        }

        // Şarkı Detay Sayfası
        public IActionResult Details(int id)
        {
            // Şarkıyı bul
            var song = _context.Songs.SingleOrDefault(s => s.Id == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // Ajax ile Dinlenme Sayısını Artırma
        [HttpPost]
        public IActionResult IncrementPlays(int songId)
        {
            var song = _context.Songs.SingleOrDefault(s => s.Id == songId);
            if (song != null)
            {
                song.Plays += 1;  // Dinlenme sayısını artır
                _context.SaveChanges();  // Veritabanına kaydet
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
