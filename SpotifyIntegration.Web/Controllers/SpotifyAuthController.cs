using SpotifyIntegration.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyIntegration.Web.Models;

namespace SpotifyIntegration.Web.Controllers;


[Authorize]
public class SpotifyAuthController : Controller
{
    private readonly SpotifyAuthService _spotifyAuthService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager; // Add a private field for SignInManager
    private readonly IConfiguration _configuration;

    public SpotifyAuthController(
        SpotifyAuthService spotifyAuthService,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager, // Inject the SignInManager
        IConfiguration configuration)
    {
        _spotifyAuthService = spotifyAuthService;
        _userManager = userManager;
        _signInManager = signInManager; // Assign the injected SignInManager to the private field
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        var redirectUri = new Uri(_configuration["Spotify:RedirectUri"]);
        var clientId = _configuration["Spotify:ClientId"];
        var loginRequest = new LoginRequest(
            redirectUri,
            clientId,
            LoginRequest.ResponseType.Code
        )
        {
            Scope = new[]
            {
                Scopes.UserReadPrivate, 
                Scopes.UserReadEmail, 
                Scopes.UserTopRead,
                Scopes.UserFollowRead,
                Scopes.PlaylistReadCollaborative,
                Scopes.PlaylistReadPrivate,
            },
            ShowDialog = true,
        };
        var authorizationUrl = loginRequest.ToUri().ToString();

        return Redirect(authorizationUrl);
    }

    public async Task<IActionResult> RefreshToken()
    {
        var accessToken = await GetSpotifyAccessTokenAsync();
        if (accessToken == null)
        {
            TempData["Error"] = "Spotify account not connected.";
            return RedirectToAction("Index");
        }
        
        var refreshToken = accessToken.RefreshToken;
        var newToken = await _spotifyAuthService.RefreshSpotifyAccessTokenAsync(refreshToken);
        // await SetClaimAsync(newToken);
        TempData["Message"] = "Successfully refreshed access to Spotify!";
        return RedirectToAction("Index");
        
    }





    
    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        try
        {
            var clientId = _configuration["Spotify:ClientId"];
            var clientSecret = _configuration["Spotify:ClientSecret"];
            var redirectUri = _configuration["Spotify:RedirectUri"];
            var response = await new OAuthClient().RequestToken(
                new AuthorizationCodeTokenRequest(
                    clientId,
                    clientSecret,
                    code,
                    new Uri(redirectUri)
                )
            );            
            
            await SetClaimAsync(response);
            TempData["Message"] = "Successfully connected to Spotify!";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Failed to connect to Spotify: " + ex.Message;
            return RedirectToAction("Index");
        }
    }

    public async Task<IActionResult> Profile()
    {
        try
        {
            var accessToken = await GetSpotifyAccessTokenAsync();


            if (accessToken == null)
            {
                TempData["Error"] = "Spotify account not connected.";
                return RedirectToAction("Index");
            }
            var claimToken = accessToken.AccessToken;
            var client = new SpotifyClient(claimToken);
            var spotifyProfile = await client.UserProfile.Current();
            var topArtists = await client.UserProfile.GetTopArtists(new UsersTopItemsRequest(TimeRange.MediumTerm));
            var topTracks = await client.UserProfile.GetTopTracks(new UsersTopItemsRequest(TimeRange.MediumTerm));
            var playlists = await client.Playlists.CurrentUsers();
            
            var playlistTracks = new List<PlaylistTrack<IPlayableItem>>();
            var artistNames = new List<string>();
            var genres = new List<string>();
            foreach (var playlist in playlists.Items)
            {
                var tracks = await client.Playlists.GetItems(playlist.Id);
                var items = tracks.Items.ToList();
                foreach (var track in items)
                {
                    if (track.Track.Type == ItemType.Track)
                    {
                        var fullTrack = (FullTrack)track.Track;
                        foreach (var artist in fullTrack.Artists)
                        {
                            var name = artist.Name;
                            if (artistNames.Contains(name)) continue;
                            
                            artistNames.Add(name);
                            var artistInfo = await client.Artists.Get(artist.Id);
                            foreach (var genre in artistInfo.Genres)
                            {
                                if (genres.Contains(genre)) continue;
                                
                                genres.Add(genre);
                            }

                        }
                    }
                }
                playlistTracks.AddRange(items);
            }
            

            // Map Spotify TopArtists and TopTracks to our ViewModel
            // Map Spotify PrivateUser to our ViewModel
            var viewModel = new SpotifyProfileViewModel
            {
                Profile = spotifyProfile,
                TopArtists = topArtists.Items.ToList(),
                TopTracks = topTracks.Items.ToList(),
                Playlists = playlists.Items.ToList(),
                PlaylistTracks = playlistTracks,
                Artists = artistNames,
                Genres = genres
            };
            
           

            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Failed to retrieve Spotify profile: " + ex.Message;
            return RedirectToAction("Index");
        }
    }
    private async Task<AuthorizationCodeTokenResponse> GetSpotifyAccessTokenAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        var spotifyAccessTokenClaim = (await _userManager.GetClaimsAsync(user))
            .FirstOrDefault(c => c.Type == "SpotifyAccessToken");

        if (spotifyAccessTokenClaim == null)
            return null;
        
        var serializedAccessToken = spotifyAccessTokenClaim.Value;
        var accessToken = JsonConvert.DeserializeObject<AuthorizationCodeTokenResponse>(serializedAccessToken);
        return accessToken;
    }

    private async Task SetClaimAsync(AuthorizationCodeTokenResponse accessToken)
    {
        // Serialize the access token for storage as a claim
        var serializedAccessToken = JsonConvert.SerializeObject(accessToken);

        // Get the currently authenticated user
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            throw new Exception("User not found.");

        // Create the new claim
        var newSpotifyAccessTokenClaim = new Claim("SpotifyAccessToken", serializedAccessToken);

        // Get the user's existing claims
        var claims = await _userManager.GetClaimsAsync(user);

        // Remove any existing claim with the same type to avoid duplication
        var existingClaim = claims.FirstOrDefault(c => c.Type == "SpotifyAccessToken");
        if (existingClaim != null)
            await _userManager.RemoveClaimAsync(user, existingClaim);

        // Add the updated claim
        await _userManager.AddClaimAsync(user, newSpotifyAccessTokenClaim);

        // Refresh the user's authentication ticket to propagate the updated claims
        // Sign the user out to clear the old claims from the authentication cookie
        await _signInManager.SignOutAsync();

        // Re-sign the user in to apply the updated claims
        await _signInManager.SignInAsync(user, isPersistent: false);
    }
    
  
}

