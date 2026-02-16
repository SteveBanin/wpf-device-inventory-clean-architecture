using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace IntegrationTests.Repositories;

public class DeviceRepositoryTests
{
    private SqliteConnection _connection = null!;
    private IDbContextFactory<AppDbContext> _dbFactory = null!;
    private DeviceRepository _repo = null!;

    [SetUp]
    public void Setup()
    {
        // Keep one SQLite in-memory connection open for the whole test.
        // If the connection closes, the in-memory DB is gone.
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            // IMPORTANT: Use the SAME open connection so every DbContext shares the same in-memory DB
            .UseSqlite(_connection)
            .Options;

        // Minimal factory for tests: creates a new DbContext per repository call
        _dbFactory = new TestDbContextFactory(options);

        // Create schema once
        using var db = _dbFactory.CreateDbContext();
        db.Database.EnsureCreated();

        _repo = new DeviceRepository(_dbFactory);
    }

    [TearDown]
    public void TearDown()
    {
        _connection.Close();
        _connection.Dispose();
    }

    [Test]
    public async Task AddAsync_ShouldPersistDevice()
    {
        var device = new Device
        {
            Name = "Laptop",
            SerialNumber = "SN-123",
            Location = "Office"
        };

        await _repo.AddAsync(device);

        var all = await _repo.GetAllAsync();

        Assert.That(all, Is.Not.Null);
        Assert.That(all.Count, Is.EqualTo(1));
        Assert.That(all[0].Name, Is.EqualTo("Laptop"));
        Assert.That(all[0].SerialNumber, Is.EqualTo("SN-123"));
        Assert.That(all[0].Location, Is.EqualTo("Office"));
    }

    /// <summary>
    /// Test-only DbContextFactory.
    /// Why we need it:
    /// - DeviceRepository now depends on IDbContextFactory<AppDbContext>
    /// - The repository creates a fresh DbContext for each operation (good for desktop apps)
    /// </summary>
    private sealed class TestDbContextFactory : IDbContextFactory<AppDbContext>
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public TestDbContextFactory(DbContextOptions<AppDbContext> options)
        {
            _options = options;
        }

        public AppDbContext CreateDbContext()
        {
            return new AppDbContext(_options);
        }
    }
}
