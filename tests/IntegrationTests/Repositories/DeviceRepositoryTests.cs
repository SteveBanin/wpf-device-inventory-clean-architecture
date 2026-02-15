using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace IntegrationTests.Repositories;

public class DeviceRepositoryTests
{
    private AppDbContext _db = null!;
    private DeviceRepository _repo = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        _db = new AppDbContext(options);
        _db.Database.OpenConnection();
        _db.Database.EnsureCreated();

        _repo = new DeviceRepository(_db);
    }

    [TearDown]
    public void TearDown()
    {
        _db.Database.CloseConnection();
        _db.Dispose();
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
}
