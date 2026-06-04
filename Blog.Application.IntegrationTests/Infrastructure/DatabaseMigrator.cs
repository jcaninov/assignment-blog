using Npgsql;

namespace Blog.Application.IntegrationTests.Infrastructure;

internal static class DatabaseMigrator
{
    private static readonly string[] MigrationFiles =
    [
        "000_enable_extensions.sql",
        "001_create_author_post.sql",
        "999_seed.sql"
    ];

    public static async Task InitializeAsync(string connectionString)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        foreach (var file in MigrationFiles)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Sql", file);
            var sql = await File.ReadAllTextAsync(path);
            await using var command = new NpgsqlCommand(sql, connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
