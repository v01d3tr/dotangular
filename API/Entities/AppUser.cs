namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    // why byte[] for password related stuff?
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}
