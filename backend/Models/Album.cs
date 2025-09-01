using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models;

public class AlbumCreate
{
    public string Name { get; set; }
    [ForeignKey("Artist")]
    public int ArtistId { get; set; }
    public DateTime ReleaseDate { get; set; }
    public int Duration { get; set; }
    public string Imagen { get; set; }
    public bool SoftDelete { get; set; }
}

public class AlbumRead : AlbumCreate
{
    [Key]
    public int Id { get; set; }
}

public class Album : AlbumRead
{
    public Artist Artist { get; set; }
    public List<Track> Tracks { get; set; }
}

