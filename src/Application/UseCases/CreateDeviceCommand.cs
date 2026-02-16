using Application.Abstractions;
using Domain.Entities;

namespace Application.Devices;

/// <summary>
/// "Command" use-case: changes data (write).
/// </summary>
public sealed class CreateDeviceCommand
{
    private readonly IDeviceRepository _repo;

    public CreateDeviceCommand(IDeviceRepository repo) => _repo = repo;

    public Task ExecuteAsync(Device device, CancellationToken ct = default)
        => _repo.AddAsync(device, ct);
}
