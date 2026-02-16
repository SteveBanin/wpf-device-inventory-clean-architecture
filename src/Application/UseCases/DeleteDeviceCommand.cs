using Application.Abstractions;

namespace Application.Devices;

public sealed class DeleteDeviceCommand
{
    private readonly IDeviceRepository _repo;

    public DeleteDeviceCommand(IDeviceRepository repo) => _repo = repo;

    public Task ExecuteAsync(int id, CancellationToken ct = default)
        => _repo.DeleteAsync(id, ct);
}
