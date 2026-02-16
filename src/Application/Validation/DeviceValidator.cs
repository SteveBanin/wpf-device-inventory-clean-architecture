using Domain.Entities;

namespace Application.Validation;

/// <summary>
/// Device validation rules live in the Application layer.
/// This keeps UI thin and makes rules reusable (API, CLI, etc.).
/// </summary>
public sealed class DeviceValidator : IDeviceValidator
{
    public ValidationResult Validate(Device device)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(device.Name))
            result.Errors["Name"] = "Name is required.";

        if (string.IsNullOrWhiteSpace(device.SerialNumber))
            result.Errors["SerialNumber"] = "Serial Number is required.";

        // Optional: add more rules as you like
        // E.g., if (device.Name.Length > 100) result.Errors["Name"] = "Name must be <= 100 chars.";

        return result;
    }
}
