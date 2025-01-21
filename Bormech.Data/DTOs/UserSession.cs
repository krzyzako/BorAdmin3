namespace Bormech.Data.DTOs;

public class UserSession
{
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
    public string? Email { get; set; }
}