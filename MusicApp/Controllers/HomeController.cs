using Microsoft.AspNetCore.Mvc;
using MusicApp.Models;
using MusicApp.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MusicApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MusicAppDbContext _context;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger, MusicAppDbContext context, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _context = context;
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
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
                return Json(new { success = false, message = "API çaðrýsýnda hata oluþtu." });
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

}
