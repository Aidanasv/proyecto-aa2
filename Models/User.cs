namespace Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



public class UserAuth
{
    public string Email { get; set; }
    public string Password { get; set; }
}
public class UserCreate : UserAuth
{
    public string Name { get; set; }
    public string Username { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastLogin { get; set; }
    [ForeignKey("TypeUser")]
    public int TypeUserId { get; set; }
}

public class UserRead : UserCreate
{
    [Key]
    public int Id { get; set; }
}
public class User : UserRead
{
    public List<Playlist> Playlists { get; set; }
    public TypeUser TypeUser { get; set; }

}

