using CoupleGame.Backend.Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoupleGame.Backend.Auth.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IGoogleService _googleService;
    private readonly IJwtService _jwtService;
    private readonly IAppleService _appleService;
    private readonly IFacebookService _facebookService;

    public AuthController(IGoogleService googleService, IJwtService jwtService, IAppleService appleService, IFacebookService facebookService)
    {
        _googleService = googleService;
        _jwtService = jwtService;
        _appleService = appleService;
        _facebookService = facebookService;
    }

    [HttpGet("google/{idToken}")]
    public async Task<IActionResult> Google([FromRoute] string idToken)
    {
        try
        {
            var payload = await _googleService.VerifyGoogleToken(idToken);

            if (payload == null)
            {
                return Unauthorized();
            }
            
            var token = _jwtService.GenerateJwtToken(payload.Subject);

            return Ok(new { token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("apple/{idToken}")]
    public async Task<IActionResult> Apple([FromRoute] string idToken)
    {
        try
        {
            var payload = await _appleService.VerifyAppleToken(idToken);

            if (payload == null)
            {
                return Unauthorized();
            }

            var userId = payload.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var token = _jwtService.GenerateJwtToken(userId);

            return Ok(new { token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("facebook/{idToken}")]
    public async Task<IActionResult> Facebook([FromRoute] string idToken)
    {
        try
        {
            var userId = await _facebookService.ValidateFacebookToken(idToken);

            if (userId == null)
            {
                return Unauthorized();
            }

            var token = _jwtService.GenerateJwtToken(userId);

            return Ok(new { token = token });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
