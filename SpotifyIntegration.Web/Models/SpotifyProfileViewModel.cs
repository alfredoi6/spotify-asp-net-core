using SpotifyAPI.Web;

namespace SpotifyIntegration.Web.Models;

public class SpotifyProfileViewModel
{
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string SpotifyUrl { get; set; }
    public string ProfileImageUrl { get; set; }
    public int FollowersCount { get; set; }
        
    public List<FullTrack> TopTracks { get; set; } = new();
    public List<FullArtist> TopArtists { get; set; } = new();
    public PrivateUser Profile { get; set; }
    
    public List<FullPlaylist> Playlists { get; set; }
    public List<PlaylistTrack<IPlayableItem>> PlaylistTracks { get; set; }
    public List<string> Artists { get; set; }
    public List<string> Genres { get; set; }
}