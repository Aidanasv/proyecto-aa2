namespace Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class MusicDbContext : DbContext
{
    public MusicDbContext(DbContextOptions<MusicDbContext> options) : base(options) { }

    public DbSet<Artist> Artists { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Playlist> Playlists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Solo configurar la cascada problemática, dejar que las DataAnnotations manejen el resto
        modelBuilder.Entity<Track>()
            .HasOne(t => t.Artist)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        // Datos de seeding con fechas fijas
        modelBuilder.Entity<User>().HasData(
            new User { 
                Id = 1, 
                Username = "admin", 
                Email = "admin@svalero.com", 
                Password = "090801", 
                Role = Role.Admin.ToString(), 
                Name = "Admin",
                BirthDate = new DateTime(1990, 1, 1),
                CreateDate = new DateTime(2025, 1, 1),
                LastLogin = new DateTime(2025, 1, 1)
            }
        );
    }

}