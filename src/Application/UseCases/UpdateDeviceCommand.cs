using Application.Abstractions;
using Domain.Entities;

namespace Application.Devices;

public sealed class UpdateDeviceCommand
{
    private readonly IDeviceRepository _repo;

    public UpdateDeviceCommand(IDeviceRepository repo) => _repo = repo;

    public Task ExecuteAsync(Device device, CancellationToken ct = default)
        => _repo.UpdateAsync(device, ct);
}
