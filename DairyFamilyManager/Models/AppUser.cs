namespace DairyFamilyManager.Models;

public class AppUser
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "DataEntry";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; }
}
