namespace SpotifyIntegration.Web.Models;

public class TrackViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string ArtistName { get; set; }
    public string AlbumName { get; set; }
    public string ImageUrl { get; set; }
    public string AvailableMarkets { get; set; }
    public int Popularity { get; set; }
    public string Url { get; set; }
    public bool Explicit { get; set; }
}