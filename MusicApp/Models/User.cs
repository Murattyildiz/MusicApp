namespace MusicApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Hashli şifre saklanır
        public string Role { get; set; }
        public bool IsActive { get; set; } // E-posta onay durumu
    }

}
