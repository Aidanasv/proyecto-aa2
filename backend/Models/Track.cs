using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class TrackCreate
{
    public string Name { get; set; }
    [ForeignKey("Artist")]
    public int ArtistId { get; set; }
    [ForeignKey("Album")]
    public int AlbumId { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int Plays { get; set; } //
    public bool SoftDelete { get; set; }
}

public class TrackRead : TrackCreate
{
    [Key]
    public int Id { get; set; }
}

public class Track : TrackRead
{
    public Artist Artist { get; set; }
    public Album Album { get; set; }
    public List<Playlist> Playlists { get; set; }
}