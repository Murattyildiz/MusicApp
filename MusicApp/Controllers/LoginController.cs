using Microsoft.AspNetCore.Mvc;
using MusicApp.Data;
using MusicApp.Helpers;
using MusicApp.Models;
using System.Linq;

namespace MusicApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly MusicAppDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(MusicAppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Giriş Sayfası
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);

            if (user != null && PasswordHelper.VerifyPassword(password, user.Password)) // Şifre doğrulama
            {
                if (!user.IsEmailConfirmed)
                {
                    ViewBag.Error = "Lütfen hesabınızı onaylayın. E-postanızı kontrol edin.";
                    return View();
                }

                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Kullanıcı adı veya şifre hatalı!";
            return View();
        }

        // Kayıt Sayfası
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string email, string password)
        {
            if (_context.Users.Any(u => u.Username == username || u.Email == email))
            {
                ViewBag.Error = "Bu kullanıcı adı veya e-posta zaten alınmış!";
                return View();
            }

            // Şifreyi hashle
            var hashedPassword = PasswordHelper.HashPassword(password);

            // Yeni kullanıcıyı oluştur
            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword, // Hashli şifreyi kaydet
                Role = "User",
                IsEmailConfirmed = false // E-posta onaylanana kadar false
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            // Hesap onay e-postası gönder
            SendAccountConfirmationEmail(user);

            ViewBag.Message = "Hesabınız oluşturuldu. Lütfen e-postanızı kontrol edin.";
            return View();
        }

        // Hesap Onaylama
        public IActionResult ConfirmEmail(int userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.IsEmailConfirmed = true;
                _context.SaveChanges();
                ViewBag.Message = "E-postanız başarıyla onaylandı. Şimdi giriş yapabilirsiniz.";
            }
            else
            {
                ViewBag.Error = "Geçersiz onay bağlantısı.";
            }

            return View();
        }

        // Şifremi Unuttum
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            if (user != null)
            {
                var resetLink = Url.Action("ResetPassword", "Login", new { userId = user.Id }, Request.Scheme);
                var emailService = new EmailService(_configuration);
                emailService.SendEmail(user.Email, "Şifre Sıfırlama", $"Şifrenizi sıfırlamak için <a href='{resetLink}'>buraya tıklayın</a>.");
            }

            ViewBag.Message = "Eğer e-posta adresiniz kayıtlı ise şifre sıfırlama bağlantısı gönderildi.";
            return View();
        }

        // Şifre Sıfırlama Sayfası
        public IActionResult ResetPassword(int userId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
            {
                ViewBag.Error = "Geçersiz bağlantı.";
                return View();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult ResetPassword(int userId, string newPassword)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user != null)
            {
                user.Password = PasswordHelper.HashPassword(newPassword);
                _context.SaveChanges();
                ViewBag.Message = "Şifreniz başarıyla sıfırlandı. Giriş yapabilirsiniz.";
            }
            else
            {
                ViewBag.Error = "Geçersiz bağlantı.";
            }

            return View();
        }

        // Çıkış Yap
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Oturum bilgisini temizle
            return RedirectToAction("Login", "Login");
        }

        // Hesap Onay E-posta Gönderimi
        private void SendAccountConfirmationEmail(User user)
        {
            var confirmationLink = Url.Action("ConfirmEmail", "Login", new { userId = user.Id }, Request.Scheme);
            var emailService = new EmailService(_configuration);
            emailService.SendEmail(user.Email, "Hesap Onaylama", $"Lütfen hesabınızı onaylamak için <a href='{confirmationLink}'>buraya tıklayın</a>.");
        }
    }
}
