using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly AppDbContext _db;

    public DeviceRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Device>> GetAllAsync(CancellationToken ct = default) =>
        _db.Devices.AsNoTracking()
            .OrderBy(d => d.Name)
            .ToListAsync(ct);

    public Task<Device?> GetByIdAsync(int id, CancellationToken ct = default) =>
        _db.Devices.FirstOrDefaultAsync(d => d.Id == id, ct);

    public async Task AddAsync(Device device, CancellationToken ct = default)
    {
        _db.Devices.Add(device);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Device device, CancellationToken ct = default)
    {
        _db.Devices.Update(device);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var existing = await _db.Devices.FirstOrDefaultAsync(d => d.Id == id, ct);
        if (existing is null) return;

        _db.Devices.Remove(existing);
        await _db.SaveChangesAsync(ct);
    }
}
