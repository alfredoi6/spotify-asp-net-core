using SpotifyAPI.Web;
using SpotifyIntegration.Web.Models;

namespace SpotifyIntegration.Web.Services;

public class SpotifyAuthService
{
    private readonly IConfiguration _configuration;

    public SpotifyAuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    

 
    public async Task<SpotifyAccessToken> RefreshSpotifyAccessTokenAsync(string refreshToken)
    {
        var clientId = _configuration["Spotify:ClientId"];
        var clientSecret = _configuration["Spotify:ClientSecret"];
        var redirectUri = _configuration["Spotify:RedirectUri"];
        var response = await new OAuthClient().RequestToken(
            new TokenSwapRefreshRequest(new Uri(redirectUri), refreshToken));
        var model = new SpotifyAccessToken
        {
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken,
            ExpiresIn = response.ExpiresIn,
            Scope = response.Scope,
            TokenType = response.TokenType,
            CreatedAt =  response.CreatedAt,
            IsExpired =  response.IsExpired
        };
        return model;
    }
    
  
}