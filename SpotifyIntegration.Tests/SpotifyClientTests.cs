using Moq;
using SpotifyAPI.Web;
using SpotifyIntegration.Web.Services;

namespace SpotifyIntegration.Tests
{
    public class SpotifyClientTests
    {
        private readonly Mock<ISpotifyClient> _mockSpotifyClient;

        public SpotifyClientTests()
        {
            _mockSpotifyClient = new Mock<ISpotifyClient>();
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

            _mockSpotifyClient.Setup(x => x.Tracks.Get(expectedTrack.Id, It.IsAny<TrackRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTrack);

            var spotifyService = new SpotifyService(_mockSpotifyClient.Object);

            // Act
            var result = await spotifyService.GetTrackInfo("1234567890");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTrack.Id, result.Id);
            Assert.Equal(expectedTrack.Name, result.Name);
            Assert.Equal(expectedTrack.Artists[0].Name, result.Name);
        }

        [Fact]
        public async Task SearchTracks_ReturnsSearchResults()
        {
            // Arrange
            var searchResponse = new SearchResponse
            {
                Tracks = new Paging<FullTrack, SearchResponse>
                {
                    Items = new List<FullTrack>() { new() { Id = "1", Name = "Track 1" }, new() { Id = "2", Name = "Track 2" } }
                }
            };

            _mockSpotifyClient.Setup(x => x.Search.Item(It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchResponse);

            var spotifyService = new SpotifyService(_mockSpotifyClient.Object);

            // Act
            var results = await spotifyService.SearchTracks("test query");

            // Assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Tracks.Items.Count);
            Assert.Equal("Track 1", results.Tracks.Items[0].Name);
            Assert.Equal("Track 2", results.Tracks.Items[1].Name);
        }
    }
}

