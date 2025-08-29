namespace Models;

public class ArtistDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Biography { get; set; }
    public string Imagen { get; set; }
    public int Followers { get; set; }
    public List<AlbumDto> Albums { get; set; }
}

public class AlbumDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Imagen { get; set; }
    public int ArtistId { get; set; }
}

public class AlbumTrackDTO : AlbumDto
{
    public List<TrackDto> Tracks { get; set; }
}