using Google.Apis.Auth;

namespace CoupleGame.Backend.Auth.Interfaces;

public interface IGoogleService
{
    Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(string idToken);
    Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string idToken);

}
