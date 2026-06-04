using Npgsql;

namespace Blog.Application.IntegrationTests.Infrastructure;

internal static class TestDataHelper
{
    public static async Task<Guid> InsertAuthorAsync(
        string connectionString,
        string name = "Jane",
        string surname = "Doe",
        CancellationToken cancellationToken = default)
    {
        var authorId = Guid.NewGuid();
        const string sql = """
            INSERT INTO author (id, name, surname, "createdAt")
            VALUES (@Id, @Name, @Surname, @CreatedAt)
            """;

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddWithValue("Id", authorId);
        command.Parameters.AddWithValue("Name", name);
        command.Parameters.AddWithValue("Surname", surname);
        command.Parameters.AddWithValue("CreatedAt", DateTime.UtcNow);
        await command.ExecuteNonQueryAsync(cancellationToken);

        return authorId;
    }

    public static async Task<bool> PostExistsAsync(
        string connectionString,
        Guid postId,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand("SELECT EXISTS(SELECT 1 FROM post WHERE id = @Id)", connection);
        command.Parameters.AddWithValue("Id", postId);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is true;
    }

    public static async Task<int> CountDomainEventsForAggregateAsync(
        string connectionString,
        Guid aggregateId,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand(
            "SELECT COUNT(*) FROM domain_events WHERE aggregate_id = @AggregateId",
            connection);
        command.Parameters.AddWithValue("AggregateId", aggregateId);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt32(result);
    }
}
