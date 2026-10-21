using GymPro.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GymPro.Persistence.Tests;

/// <summary>
/// A real SQLite database per test, kept open for the lifetime of the fixture so
/// the schema is never silently discarded. Tests exercise the actual mappings,
/// converters and constraints rather than an in-memory stand-in.
/// </summary>
public sealed class SqliteHarness : IAsyncDisposable
{
    private readonly SqliteConnection _connection;

    public SqliteHarness()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<GymDbContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new GymDbContext(options);
        Context.Database.EnsureCreated();
    }

    public GymDbContext Context { get; }

    public GymDbContext NewContext()
    {
        var options = new DbContextOptionsBuilder<GymDbContext>()
            .UseSqlite(_connection)
            .Options;
        return new GymDbContext(options);
    }

    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
