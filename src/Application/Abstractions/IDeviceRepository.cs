using Domain.Entities;

namespace Application.Abstractions;

public interface IDeviceRepository
{
    Task<List<Device>> GetAllAsync(CancellationToken ct = default);
    Task<Device?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Device device, CancellationToken ct = default);
    Task UpdateAsync(Device device, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
