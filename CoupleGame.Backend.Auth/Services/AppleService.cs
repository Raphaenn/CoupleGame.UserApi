using CoupleGame.Backend.Auth.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoupleGame.Backend.Auth.Services;

public class AppleService : IAppleService
{
    private readonly string AppleIssuer;
    private readonly string AppleClientId;
    private readonly string Secret;

    public AppleService(IConfiguration configuration)
    {
        AppleIssuer = Environment.GetEnvironmentVariable("APPLE_ISSUER") ?? configuration.GetValue<string>("Apple:Issuer");
        AppleClientId = Environment.GetEnvironmentVariable("APPLE_CLIENT_ID") ?? configuration.GetValue<string>("Apple:ClientId");
        Secret = Environment.GetEnvironmentVariable("APPLE_SECRET") ?? configuration.GetValue<string>("Apple:Secret");
    }

    public async Task<JwtSecurityToken> ValidateAppleToken(string idToken) => await Task.Run(() =>
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(idToken) as JwtSecurityToken;

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AppleIssuer,
                ValidAudience = AppleClientId,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret))
            };

            handler.ValidateToken(idToken, validationParameters, out SecurityToken validatedToken);

            return validatedToken as JwtSecurityToken;
        }
        catch
        {
            return null;
        }
    });

    public async Task<ClaimsPrincipal> VerifyAppleToken(string idToken) => await Task.Run(() =>
    {
        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(idToken) as JwtSecurityToken;

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = AppleIssuer,
                ValidAudience = AppleClientId,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret))
            };

            return handler.ValidateToken(idToken, validationParameters, out var validatedToken);
        }
        catch
        {
            return null;
        }
    });

}
