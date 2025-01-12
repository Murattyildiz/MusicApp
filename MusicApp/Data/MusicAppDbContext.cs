using Microsoft.EntityFrameworkCore;
using MusicApp.Models;
using System.Collections.Generic;

namespace MusicApp.Data
{
    public class MusicAppDbContext : DbContext
    {
        public MusicAppDbContext(DbContextOptions<MusicAppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PlaylistSong> PlaylistSongs { get; set; }
    }
}
