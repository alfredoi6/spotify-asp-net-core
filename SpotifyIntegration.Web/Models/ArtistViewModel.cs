namespace SpotifyIntegration.Web.Models;

public class ArtistViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? ImageUrl { get; set; }
    public int Popularity { get; set; }
    public string Url { get; set; }
    public string Genres { get; set; }
}