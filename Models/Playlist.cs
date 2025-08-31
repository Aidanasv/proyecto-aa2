using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class PlaylistCreate
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool SoftDelete { get; set; }
}

public class PlaylistRead : PlaylistCreate
{
    [Key]
    public int Id { get; set; }
}
public class Playlist : PlaylistRead
{
    public User User { get; set; }
    [ForeignKey("User")]
    public int UserId { get; set; }

    public List<Track> Tracks { get; set; }
}