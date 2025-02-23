using SpotifyAPI.Web;

public interface ISpotifyService
{
    Task<FullTrack> GetTrackInfo(string trackId);
    Task<SearchResponse> SearchTracks(string query);
    Task<PrivateUser> GetUserProfile();
}