using SpotifyIntegration.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SpotifyIntegration.Web.Models;

namespace SpotifyIntegration.Web.Controllers;


[Authorize]
public class SpotifyAuthController : Controller
{
    private readonly SpotifyAuthService _spotifyAuthService;
    private readonly SpotifyService _spotifyService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public SpotifyAuthController(
        SpotifyAuthService spotifyAuthService,
        SpotifyService spotifyService,
        UserManager<IdentityUser> userManager,
        IConfiguration configuration)
    {
        _spotifyAuthService = spotifyAuthService;
        _spotifyService = spotifyService;
        _userManager = userManager;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        var authorizationUrl = _spotifyAuthService.GetAuthorizationUrl();
        return Redirect(authorizationUrl);
    }

    public async Task<IActionResult> Callback([FromQuery] string code)
    {
        try
        {
            var spotifyClient = await _spotifyAuthService.GetSpotifyClientAsync(code);
            var profile = await spotifyClient.UserProfile.Current();

            // Store the Spotify user ID with the current user
            var user = await _userManager.GetUserAsync(User);
            var spotifyUserIdClaim = new Claim("SpotifyUserId", profile.Id);
            await _userManager.AddClaimAsync(user, spotifyUserIdClaim);

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
            var user = await _userManager.GetUserAsync(User);
            var spotifyUserIdClaim = (await _userManager.GetClaimsAsync(user))
                .FirstOrDefault(c => c.Type == "SpotifyUserId");

            if (spotifyUserIdClaim == null)
            {
                TempData["Error"] = "Spotify account not connected.";
                return RedirectToAction("Index");
            }

            var spotifyProfile = await _spotifyService.GetUserProfile();
        
            // Map Spotify PrivateUser to our ViewModel
            var viewModel = new SpotifyProfileViewModel
            {
                DisplayName = spotifyProfile.DisplayName,
                Email = spotifyProfile.Email,
                Country = spotifyProfile.Country,
                SpotifyUrl = spotifyProfile.ExternalUrls.GetValueOrDefault("spotify"),
                ProfileImageUrl = spotifyProfile.Images.FirstOrDefault()?.Url,
                FollowersCount = spotifyProfile.Followers.Total
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Failed to retrieve Spotify profile: " + ex.Message;
            return RedirectToAction("Index");
        }
    }
}

