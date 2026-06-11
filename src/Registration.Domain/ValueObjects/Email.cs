using System.Text.RegularExpressions;
using Registration.Domain.Exceptions;

namespace Registration.Domain.ValueObjects;

/// <summary>
/// Represents an email address. Stores both the original (trimmed) value and a
/// normalized (lowercased) value used for case-insensitive equality and uniqueness checks.
/// </summary>
public sealed partial class Email : Common.ValueObject
{
    public const int MaxLength = 254;

    public string Value { get; }

    public string NormalizedValue { get; }

    private Email(string value, string normalizedValue)
    {
        Value = value;
        NormalizedValue = normalizedValue;
    }

    public static Email Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Email is required.");
        }

        var trimmed = value.Trim();

        if (trimmed.Length > MaxLength)
        {
            throw new DomainException($"Email must not exceed {MaxLength} characters.");
        }

        if (!EmailRegex().IsMatch(trimmed))
        {
            throw new DomainException("Email format is invalid.");
        }

        return new Email(trimmed, trimmed.ToLowerInvariant());
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
    private static partial Regex EmailRegex();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return NormalizedValue;
    }

    public override string ToString() => Value;
}
