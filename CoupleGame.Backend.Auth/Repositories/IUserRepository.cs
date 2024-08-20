using CoupleGame.Backend.Auth.Model;

namespace CoupleGame.Backend.Auth.Repositories;

public interface IUserRepository
{
    Task<Users> GetUsersByExternalIdAsync(string externalId);
    Task SaveUser(Users user);

}
