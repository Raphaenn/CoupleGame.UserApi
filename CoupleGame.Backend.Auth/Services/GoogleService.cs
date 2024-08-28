using CoupleGame.Backend.Auth.Interfaces;
using Google.Apis.Auth;

namespace CoupleGame.Backend.Auth.Services;

public class GoogleService : IGoogleService
{
    private readonly string GoogleClientId;

    public GoogleService(IConfiguration configuration)
    {
        GoogleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLOUD_ID") ?? configuration.GetValue<string>("Google:GoogleClientId");
    }

    public async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new[] { GoogleClientId }
            };

            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
        catch (InvalidJwtException)
        {
            return null;
        }
    }

    public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { GoogleClientId }
            };

            return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
        }
        catch 
        {
            return null;
        }
    }
}
