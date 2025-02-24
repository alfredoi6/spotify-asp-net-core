namespace SpotifyIntegration.Web.Models;

public class SpotifyAccessToken
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
    public string Scope { get; set; }
    public string TokenType { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsExpired { get; set; }
}