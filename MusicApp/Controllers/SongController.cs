using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
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
        public IActionResult Index()
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

            // Şarkıları ve kullanıcıya ait çalma listelerini getir
            var songs = _context.Songs.ToList();
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
    }
}
