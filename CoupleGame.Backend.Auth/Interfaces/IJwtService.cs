namespace CoupleGame.Backend.Auth.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(string userId);
}
