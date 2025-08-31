namespace Models;

using System.ComponentModel.DataAnnotations;

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
    public string Role { get; set; }
}

public class UserRead : UserCreate
{
    [Key]
    public int Id { get; set; }
}
public class User : UserRead
{
    public List<Playlist> Playlists { get; set; }

}

public class UserToken
{
    public int Id { get; set; }
    public string Role { get; set; }
}
