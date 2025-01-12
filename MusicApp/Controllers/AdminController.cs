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

        // Action çalıştırılmadan önce kontrol
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
            {
                context.Result = RedirectToAction("Index", "Home");
            }

            base.OnActionExecuting(context);
        }

        // Şarkıların Listelendiği Sayfa
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
        public IActionResult Create(Song song, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/Sarkilar", file.FileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(stream);
                song.FilePath = $"Sarkilar/{file.FileName}";
            }

            _context.Songs.Add(song);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Şarkı Düzenleme
        public IActionResult Edit(int id)
        {
            var song = _context.Songs.Find(id);
            if (song == null)
                return NotFound();

            return View(song);
        }

        [HttpPost]
        public IActionResult Edit(Song song, IFormFile file)
        {
            var existingSong = _context.Songs.Find(song.Id);
            if (existingSong != null)
            {
                existingSong.Title = song.Title;
                existingSong.Artist = song.Artist;
                existingSong.Album = song.Album;
                existingSong.Genre = song.Genre;

                if (file != null && file.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/Sarkilar", file.FileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    file.CopyTo(stream);
                    existingSong.FilePath = $"Sarkilar/{file.FileName}";
                }

                _context.SaveChanges();
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

            return RedirectToAction("Index");
        }
    }
}
