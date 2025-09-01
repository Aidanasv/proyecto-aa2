using System.ComponentModel.DataAnnotations;

namespace Models;

public class ArtistCreate
{
    public string Name { get; set; }
    public int Followers { get; set; }
    public string Biography { get; set; }
    public DateTime CreateDate { get; set; }
    public string Imagen { get; set; }
    public bool SoftDelete { get; set; }
   
}

public class ArtistRead : ArtistCreate
{
    [Key]
    public int Id { get; set; }
}

public class Artist : ArtistRead
{
    public List<Album> Albums { get; set; }
}
