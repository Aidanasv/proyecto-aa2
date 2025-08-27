using Microsoft.EntityFrameworkCore;
using Models;

namespace Data;

public class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options) { }

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Playlist> Playlists { get; set; }
    public DbSet<TypeUser> TypeUsers { get; set; }
}