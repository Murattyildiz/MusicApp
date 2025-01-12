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
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Index", "Login");

            var songs = _context.Songs.ToList();
            var playlists = _context.Playlists.Where(p => p.UserId == user.Id).ToList();

            ViewBag.Playlists = playlists; // Çalma listelerini ViewBag'e ekle
            return View(songs);
        }

    }
}
