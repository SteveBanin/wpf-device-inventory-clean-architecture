using Application.Abstractions;
using Domain.Entities;

namespace Application.Devices;

/// <summary>
/// "Query" use-case: read data (no side effects besides reading).
/// UI calls this, and it calls the repository abstraction.
/// </summary>
public sealed class GetDevicesQuery
{
    private readonly IDeviceRepository _repo;

    public GetDevicesQuery(IDeviceRepository repo) => _repo = repo;

    public Task<List<Device>> ExecuteAsync(CancellationToken ct = default)
        => _repo.GetAllAsync(ct);
}
