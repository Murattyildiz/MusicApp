using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Models;
using System.Linq;

namespace MusicApp.Controllers
{
    public class FavoriteController : Controller
    {
        private readonly MusicAppDbContext _context;

        public FavoriteController(MusicAppDbContext context)
        {
            _context = context;
        }

        // Favorilerim Sayfası
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Login");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Index", "Login");

            var favorites = _context.Favorites
                .Where(f => f.UserId == user.Id)
                .Select(f => f.Song)
                .ToList();

            return View(favorites);
        }

        // Favorilere Ekle
        [HttpPost]
        public IActionResult AddToFavorites(int songId)
        {
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Login");
            }

            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            if (user == null) return RedirectToAction("Index", "Login");

            var alreadyFavorite = _context.Favorites.Any(f => f.UserId == user.Id && f.SongId == songId);
            if (!alreadyFavorite)
            {
                var favorite = new Favorite { UserId = user.Id, SongId = songId };
                _context.Favorites.Add(favorite);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Song");
        }

        // Favorilerden Kaldır
        [HttpPost]
        public IActionResult RemoveFromFavorites(int favoriteId)
        {
            var favorite = _context.Favorites.SingleOrDefault(f => f.Id == favoriteId);
            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
