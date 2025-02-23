namespace SpotifyIntegration.Web.Models;

public class SpotifyProfileViewModel
{
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string SpotifyUrl { get; set; }
    public string ProfileImageUrl { get; set; }
    public int FollowersCount { get; set; }
}