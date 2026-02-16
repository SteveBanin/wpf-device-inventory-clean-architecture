using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Validation;

/// <summary>
/// Abstraction so UI depends on "a validator" not a concrete implementation.
/// This is a clean architecture-friendly approach.
/// </summary>
public interface IDeviceValidator
{
    ValidationResult Validate(Device device);
}
