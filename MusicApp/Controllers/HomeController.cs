using Microsoft.AspNetCore.Mvc;
using MusicApp.Models;
using MusicApp.Data; // DbContext'in doðru import edilmesi gerekir
using System.Linq;

namespace MusicApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MusicAppDbContext _context;  // DbContext'i buraya ekliyoruz

        public HomeController(ILogger<HomeController> logger, MusicAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // En çok dinlenen þarkýlarý alýyoruz (Plays'e göre sýralama)
            var mostPlayedSongs = _context.Songs
                                          .OrderByDescending(s => s.Plays)  // Plays sayýsýna göre azalan sýralama
                                          .Take(5)  // Ýlk 5 þarkýyý alýyoruz
                                          .ToList();

            // View'a gönderilecek model
            return View(mostPlayedSongs);  // `mostPlayedSongs` ile view'ý döndürüyoruz
        }
    }
}
