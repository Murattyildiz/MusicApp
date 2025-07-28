namespace MusicApp.Models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public string FilePath { get; set; } 
        public string CoverImagePath { get; set; } 
        public string Lyrics { get; set; } 
        public int Plays { get; set; } 
    }
}
