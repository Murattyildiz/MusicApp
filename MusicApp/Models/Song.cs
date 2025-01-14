namespace MusicApp.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public string FilePath { get; set; } // Dosya yolu
        public string CoverImagePath { get; set; } // Kapak fotoğrafı yolu
        public string Lyrics { get; set; } // Şarkı sözleri

        // Dinlenme sayısını tutan alan
        public int Plays { get; set; }  // Şarkının dinlenme sayısı
    }
}
