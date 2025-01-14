using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Models;
using System.Linq;

namespace MusicApp.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly MusicAppDbContext _context;

        public PlaylistController(MusicAppDbContext context)
        {
            _context = context;
        }

        // Çalma Listesi Sayfası
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Login");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Login", "Login");

            var playlists = _context.Playlists
                .Where(p => p.UserId == user.Id)
                .ToList();

            return View(playlists);
        }

        // Yeni Çalma Listesi Oluştur
        [HttpPost]
        public IActionResult Create(string playlistName)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Index", "Login");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Index", "Login");

            var playlist = new Playlist
            {
                Name = playlistName,
                UserId = user.Id
            };

            _context.Playlists.Add(playlist);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // Çalma Listesine Şarkı Ekle
        [HttpPost]
        public IActionResult AddSong(int playlistId, int songId)
        {
            var playlistSong = new PlaylistSong
            {
                PlaylistId = playlistId,
                SongId = songId
            };

            _context.PlaylistSongs.Add(playlistSong);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = playlistId });
        }

        // Çalma Listesi Detayları
        public IActionResult Details(int id)
        {
            var playlist = _context.Playlists
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    Playlist = p,
                    Songs = p.PlaylistSongs.Select(ps => ps.Song)
                })
                .SingleOrDefault();

            if (playlist == null) return NotFound();

            ViewBag.Playlist = playlist.Playlist;
            return View(playlist.Songs);
        }
    }
}
