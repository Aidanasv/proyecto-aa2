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
        modelBuilder.Entity<Track>()
            .HasOne(t => t.Artist)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@svalero.com",
                Password = "0311A011F55525D91458383CE73908F7C531C4020E479A01757A1FCB5F827BF16758A0B497C6E94540E49806525DC8ACE2173D12EE616FF0AD06D7CE628C9490",
                Role = Role.Admin.ToString(),
                Name = "Admin",
                BirthDate = new DateTime(1990, 1, 1),
                CreateDate = new DateTime(2025, 1, 1),
                LastLogin = new DateTime(2025, 1, 1)
            }
        );
    }

}