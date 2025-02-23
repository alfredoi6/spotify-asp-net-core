using SpotifyAPI.Web;

namespace SpotifyIntegration.Web.Services;

public class SpotifyService : ISpotifyService
{
    private readonly ISpotifyClient _spotifyClient;

    public SpotifyService(ISpotifyClient spotifyClient)
    {
        _spotifyClient = spotifyClient;
    }

    public async Task<FullTrack> GetTrackInfo(string trackId)
    {
        return await _spotifyClient.Tracks.Get(trackId);
    }

    public async Task<SearchResponse> SearchTracks(string query)
    {
        var request = new SearchRequest(SearchRequest.Types.Track, query);
        return await _spotifyClient.Search.Item(request);
    }

    public async Task<PrivateUser> GetUserProfile()
    {
        return await _spotifyClient.UserProfile.Current();
    }
}