using Microsoft.AspNetCore.Mvc;
using MusicApp.Models;
<<<<<<< HEAD
using MusicApp.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
=======
using MusicApp.Data; // DbContext'in do�ru import edilmesi gerekir
using System.Linq;
>>>>>>> a8992dcc3d1d2726c590c43ccae51b59f3688166

namespace MusicApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
<<<<<<< HEAD
        private readonly MusicAppDbContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, MusicAppDbContext context, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _context = context;
            _clientFactory = clientFactory;
=======
        private readonly MusicAppDbContext _context;  // DbContext'i buraya ekliyoruz

        public HomeController(ILogger<HomeController> logger, MusicAppDbContext context)
        {
            _logger = logger;
            _context = context;
>>>>>>> a8992dcc3d1d2726c590c43ccae51b59f3688166
        }

        public IActionResult Index()
        {
<<<<<<< HEAD
            var mostPlayedSongs = _context.Songs
                                          .OrderByDescending(s => s.Plays)
                                          .Take(5)
                                          .ToList();

            return View(mostPlayedSongs);
        }

        [HttpGet]
        public async Task<IActionResult> GetRecommendations()
        {
            var top3Songs = _context.Songs
                                   .OrderByDescending(s => s.Plays)
                                   .Take(3)
                                   .Select(s => new
                                   {
                                       title = s.Title,
                                       artist = s.Artist,
                                       genre = s.Genre
                                   }).ToList();

            var apiUrl = "http://127.0.0.1:8000/recommend";

            var client = _clientFactory.CreateClient();

            var requestBody = new
            {
                songs = top3Songs,
                limit = 5
            };

                var response = await client.PostAsJsonAsync(apiUrl, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var recommendationResult = JsonConvert.DeserializeObject<RecommendationResult>(jsonString);

                return Json(new { success = true, recommendations = recommendationResult.Recommendations });
            }
            else
            {
                return Json(new { success = false, message = "API �a�r�s�nda hata olu�tu." });
            }
        }
        public IActionResult Recommend()
        {
            return View();
        }

    }

    public class RecommendationResult
    {
        [JsonProperty("recommendations")]
        public List<Song> Recommendations { get; set; }
    }

=======
            // En �ok dinlenen �ark�lar� al�yoruz (Plays'e g�re s�ralama)
            var mostPlayedSongs = _context.Songs
                                          .OrderByDescending(s => s.Plays)  // Plays say�s�na g�re azalan s�ralama
                                          .Take(5)  // �lk 5 �ark�y� al�yoruz
                                          .ToList();

            // View'a g�nderilecek model
            return View(mostPlayedSongs);  // `mostPlayedSongs` ile view'� d�nd�r�yoruz
        }
    }
>>>>>>> a8992dcc3d1d2726c590c43ccae51b59f3688166
}
