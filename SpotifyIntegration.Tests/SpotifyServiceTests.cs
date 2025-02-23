using SpotifyIntegration.Web.Services;
using Moq;
using SpotifyAPI.Web;

namespace SpotifyIntegration.Tests;

public class SpotifyServiceTests
{
    private readonly Mock<ISpotifyClient> _mockSpotifyClient;
    private readonly SpotifyService _spotifyService;

    public SpotifyServiceTests()
    {
        _mockSpotifyClient = new Mock<ISpotifyClient>();
        _spotifyService = new SpotifyService(_mockSpotifyClient.Object);
    }

    [Fact]
    public async Task GetTrack_ReturnsTrackInfo()
    {
        // Arrange
        var expectedTrack = new FullTrack
        {
            Id = "1234567890",
            Name = "Test Track",
            Artists = [new SimpleArtist { Name = "Test Artist" }]
        };

        _mockSpotifyClient.Setup(x => x.Tracks.Get(It.IsAny<string>(), It.IsAny<TrackRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedTrack);

        // Act
        var result = await _spotifyService.GetTrackInfo("1234567890");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedTrack.Id, result.Id);
        Assert.Equal(expectedTrack.Name, result.Name);
        Assert.Equal(expectedTrack.Artists[0].Name, result.Artists[0].Name);
    }

    [Fact]
    public async Task SearchTracks_ReturnsSearchResults()
    {
        // Arrange
        var searchResponse = new SearchResponse
        {
            Tracks = new Paging<FullTrack, SearchResponse>
            {
                Items = [new FullTrack() { Id = "1", Name = "Track 1" }, new FullTrack { Id = "2", Name = "Track 2" }]
            }
        };

        _mockSpotifyClient.Setup(x => x.Search.Item(It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(searchResponse);

        // Act
        var results = await _spotifyService.SearchTracks("test query");

        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Tracks.Items.Count);
        Assert.Equal("Track 1", results.Tracks.Items[0].Name);
        Assert.Equal("Track 2", results.Tracks.Items[1].Name);
    }

    [Fact]
    public async Task GetUserProfile_ReturnsUserProfile()
    {
        // Arrange
        var expectedProfile = new PrivateUser
        {
            Id = "user123",
            DisplayName = "Test User",
            Email = "test@example.com"
        };

        _mockSpotifyClient.Setup(x => x.UserProfile.Current( It.IsAny<CancellationToken>() ))
            .ReturnsAsync(expectedProfile);

        // Act
        var result = await _spotifyService.GetUserProfile();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProfile.Id, result.Id);
        Assert.Equal(expectedProfile.DisplayName, result.DisplayName);
        Assert.Equal(expectedProfile.Email, result.Email);
    }
}