using CoupleGame.Backend.Auth.Interfaces;
using CoupleGame.Backend.Auth.Model;
using Newtonsoft.Json.Linq;

namespace CoupleGame.Backend.Auth.Services;

public class FacebookService : IFacebookService
{
    public async Task<FacebookUserInfo> GetFacebookUserInfo(string accessToken)
    {
        try
        {
            using var httpClient = new HttpClient();
            var userInfoUrl = $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}";

            var response = await httpClient.GetStringAsync(userInfoUrl);
            var json = JObject.Parse(response);

            var userInfo = new FacebookUserInfo
            {
                Id = json["id"]?.ToString(),
                Name = json["name"]?.ToString(),
                Email = json["email"]?.ToString()
            };

            return userInfo;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<string> ValidateFacebookToken(string accessToken)
    {
        try
        {
            using var httpClient = new HttpClient();
            var userInfoUrl = $"https://graph.facebook.com/me?access_token={accessToken}";

            var response = await httpClient.GetStringAsync(userInfoUrl);
            var json = JObject.Parse(response);

            var userId = json["id"]?.ToString();

            return userId;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
