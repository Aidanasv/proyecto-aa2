namespace Models;

public class PlaylistDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<TrackDto> Tracks { get; set; }
}

public class TrackDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Duration { get; set; }
}

public class PlaylistDtoParameters
{
    public string? Name { get; set; }
    public bool? NameOrder { get; set; }
    public string? Description { get; set; }
    public bool? DescriptionOrder { get; set; }
}
