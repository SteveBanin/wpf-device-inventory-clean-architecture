using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Why DbContextFactory?
/// - In desktop apps, DbContext lifetime can accidentally become "too long".
/// - EF tracks entities per DbContext instance.
/// - Factory creates a fresh DbContext per repository call, avoiding tracking conflicts.
/// </summary>
public sealed class DeviceRepository : IDeviceRepository
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public DeviceRepository(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task<List<Device>> GetAllAsync(CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        return await db.Devices
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .ThenBy(d => d.Id)
            .ToListAsync(ct);
    }

    public async Task<Device?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        return await db.Devices
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, ct);
    }

    public async Task AddAsync(Device device, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        db.Devices.Add(device);
        await db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Device device, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var existing = await db.Devices.FindAsync(new object[] { device.Id }, ct);
        if (existing is null) return;

        db.Entry(existing).CurrentValues.SetValues(device);
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        var existing = await db.Devices.FirstOrDefaultAsync(d => d.Id == id, ct);
        if (existing is null) return;

        db.Devices.Remove(existing);
        await db.SaveChangesAsync(ct);
    }
}
