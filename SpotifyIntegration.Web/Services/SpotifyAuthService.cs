using Microsoft.Extensions.Configuration;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Threading.Tasks;

public class SpotifyAuthService
{
    private readonly IConfiguration _configuration;
    private static EmbedIOAuthServer _server;

    public SpotifyAuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetAuthorizationUrl()
    {
        var loginRequest = new LoginRequest(
            new Uri(_configuration["Spotify:RedirectUri"]),
            _configuration["Spotify:ClientId"],
            LoginRequest.ResponseType.Code
        )
        {
            Scope = new[] { Scopes.UserReadPrivate, Scopes.UserReadEmail }
        };

        return loginRequest.ToUri().ToString();
    }

    public async Task<ISpotifyClient> GetSpotifyClientAsync(string code)
    {
        var response = await new OAuthClient().RequestToken(
            new AuthorizationCodeTokenRequest(
                _configuration["Spotify:ClientId"],
                _configuration["Spotify:ClientSecret"],
                code,
                new Uri(_configuration["Spotify:RedirectUri"])
            )
        );

        return new SpotifyClient(response.AccessToken);
    }
}