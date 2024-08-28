using CoupleGame.Backend.Auth.Context;
using CoupleGame.Backend.Auth.Model;
using Npgsql;
using System.Xml.Linq;

namespace CoupleGame.Backend.Auth.Repositories;

public class UserRepository : IUserRepository
{
    private readonly StoreDataContext _context;

    public UserRepository(StoreDataContext context)
    {
        _context = context;
    }


    public async Task<Users> GetUsersByExternalIdAsync(string externalId)
    {
        await using (var conn = await _context.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM users_view WHERE externalId = @externalId";
                command.Parameters.AddWithValue("@externalId", externalId);

                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    string idPlataform = (string)reader["externalId"];
                    string name = (string)reader["name"];
                    string email = (string)reader["email"];

                    Users user = new Users
                    {
                        ExternalId = idPlataform,
                        Name = name,
                        Email = email,
                    };

                    return user;
                }
            }
            return null;
        }
    }

    public async Task SaveUser(Users user)
    {
        await using (var conn = await _context.DataSource.OpenConnectionAsync())
        {
            await using (var command = new NpgsqlCommand("SELECT save_user(@name, @email, @externalId)", conn))
            {
                command.Parameters.AddWithValue("@name", user.Name);
                command.Parameters.AddWithValue("@externalId", user.ExternalId);
                command.Parameters.AddWithValue("@email", user.Email);

                await command.ExecuteScalarAsync();
            }
        }
    }
}
