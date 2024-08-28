using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CoupleGame.Backend.Auth.Interfaces;

public interface IAppleService
{
    Task<ClaimsPrincipal> VerifyAppleToken(string idToken);
    Task<JwtSecurityToken> ValidateAppleToken(string idToken);
}
