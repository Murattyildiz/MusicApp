using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Models; // Song modelini ekledik
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

            // Şarkıları getir
            var songs = _context.Songs.ToList();

            // Dinlenme sayısını artırıyoruz
            foreach (var song in songs)
            {
                song.Plays += 1;  // Dinlenme sayısını artır
            }

            // Veritabanına kaydediyoruz
            _context.SaveChanges();

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

            // Dinlenme sayısını artırıyoruz
            song.Plays += 1;  // Dinlenme sayısını bir artır
            _context.SaveChanges();  // Veritabanına kaydet

            return View(song);
        }
    }
}
