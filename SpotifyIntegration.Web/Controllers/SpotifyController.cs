using Microsoft.AspNetCore.Mvc;
using SpotifyIntegration.Web.Services;

namespace SpotifyIntegration.Web.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class SpotifyController : ControllerBase
    {
        private readonly ISpotifyService _spotifyService;
        private readonly SpotifyAuthService _authService;

        public SpotifyController(ISpotifyService spotifyService, SpotifyAuthService authService)
        {
            _spotifyService = spotifyService;
            _authService = authService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var authorizationUrl = _authService.GetAuthorizationUrl();
            return Redirect(authorizationUrl);
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback([FromQuery] string code)
        {
            var spotifyClient = await _authService.GetSpotifyClientAsync(code);
            // Here you would typically store the access token or client in a session or database
            // For simplicity, we'll just return it
            return Ok("Authentication successful. You can now use the API.");
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var profile = await _spotifyService.GetUserProfile();
            return Ok(profile);
        }

        [HttpGet("track/{id}")]
        public async Task<IActionResult> GetTrack(string id)
        {
            var track = await _spotifyService.GetTrackInfo(id);
            return Ok(track);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchTracks([FromQuery] string query)
        {
            var results = await _spotifyService.SearchTracks(query);
            return Ok(results);
        }
    }


