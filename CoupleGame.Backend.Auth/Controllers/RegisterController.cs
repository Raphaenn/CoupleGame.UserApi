using CoupleGame.Backend.Auth.Interfaces;
using CoupleGame.Backend.Auth.Model;
using CoupleGame.Backend.Auth.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CoupleGame.Backend.Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegisterController : Controller
{
    private readonly IGoogleService _googleService;
    private readonly IJwtService _jwtService;
    private readonly IAppleService _appleService;
    private readonly IFacebookService _facebookService;
    private readonly IUserRepository _userRepository;

    public RegisterController(IGoogleService googleService, IJwtService jwtService, IAppleService appleService, IFacebookService facebookService, IUserRepository userRepository)
    {
        _googleService = googleService;
        _jwtService = jwtService;
        _appleService = appleService;
        _facebookService = facebookService;
        _userRepository = userRepository;
    }

    [HttpPost("facebook/{idToken}")]
    public async Task<IActionResult> Facebook([FromRoute] string accessToken)
    {
        try
        {
            var payload = await _facebookService.GetFacebookUserInfo(accessToken);

            if (payload == null)
            {
                return Unauthorized();
            }

            Users existingUser = await _userRepository.GetUsersByExternalIdAsync(payload.Id);

            if (existingUser != null)
            {
                return BadRequest(new { Message = "El usuario ya está registrado." });
            }

            var newUser = new Users
            {
                ExternalId = payload.Id,
                Name = payload.Name,
                Email = payload.Email,
            };

            await _userRepository.SaveUser(newUser);

            var token = _jwtService.GenerateJwtToken(newUser.ExternalId);

            return Ok(new { token = token, user = newUser });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("google/{idToken}")]
    public async Task<IActionResult> Google([FromRoute] string idToken)
    {
        try
        {
            var payload = await _googleService.ValidateGoogleToken(idToken);

            if (payload == null)
            {
                return Unauthorized();
            }

            Users existingUser = await _userRepository.GetUsersByExternalIdAsync(payload.Subject);

            if (existingUser != null)
            {
                return BadRequest(new { Message = "El usuario ya está registrado." });
            }

            var newUser = new Users
            {
                ExternalId = payload.Subject,
                Name = payload.Name,
                Email = payload.Email,
            };

            await _userRepository.SaveUser(newUser);

            var token = _jwtService.GenerateJwtToken(newUser.ExternalId);

            return Ok(new { token = token, user = newUser });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("apple/{idToken}")]
    public async Task<IActionResult> Apple([FromRoute] string idToken)
    {
        try
        {
            var payload = await _appleService.ValidateAppleToken(idToken);

            if (payload == null)
            {
                return Unauthorized();
            }


            Users existingUser = await _userRepository.GetUsersByExternalIdAsync(payload.Subject);

            if (existingUser != null)
            {
                return BadRequest(new { Message = "El usuario ya está registrado." });
            }

            var newUser = new Users
            {
                ExternalId = payload.Subject,
                Email = payload.Claims.FirstOrDefault(c => c.Type == "email")?.Value,
                Name = payload.Claims.FirstOrDefault(c => c.Type == "email")?.Value // Si Apple devuelve el nombre, si no, maneja esto de acuerdo a tus necesidades
            };

            await _userRepository.SaveUser(newUser);

            var token = _jwtService.GenerateJwtToken(newUser.ExternalId);

            return Ok(new { token = token, user = newUser });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

}
