using Npgsql;

namespace CoupleGame.Backend.Auth.Context;

public class StoreDataContext : IAsyncDisposable
{
    private readonly NpgsqlDataSource _connection;

    public StoreDataContext(IConfiguration configuration)
    {
        string connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        _connection = NpgsqlDataSource.Create(connectionString);
    }
    public NpgsqlDataSource DataSource => _connection;

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }
}
