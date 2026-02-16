using System.Collections.Generic;

namespace Application.Validation;

/// <summary>
/// Simple validation result object.
/// UI can read errors by key (e.g. "Name", "SerialNumber").
/// </summary>
public sealed class ValidationResult
{
    public Dictionary<string, string> Errors { get; } = new();

    public bool IsValid => Errors.Count == 0;

    public string? Get(string key)
        => Errors.TryGetValue(key, out var msg) ? msg : null;
}
