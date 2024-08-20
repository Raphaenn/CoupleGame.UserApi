using CoupleGame.Backend.Auth.Model;

namespace CoupleGame.Backend.Auth.Interfaces;

public interface IFacebookService
{
    Task<string> ValidateFacebookToken(string accessToken);
    Task<FacebookUserInfo> GetFacebookUserInfo(string accessToken);
}
