using System.Text.RegularExpressions;
using Registration.Domain.Exceptions;

namespace Registration.Domain.ValueObjects;

/// <summary>
/// Represents a person's name component (first, middle, or last name).
/// Allows Arabic and Latin letters, spaces, apostrophes and hyphens, max 50 chars.
/// </summary>
public sealed partial class PersonName : Common.ValueObject
{
    public const int MaxLength = 50;

    public string Value { get; }

    private PersonName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a required name component (e.g., FirstName, LastName).
    /// </summary>
    public static PersonName Create(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"{fieldName} is required.");
        }

        var normalized = Normalize(value);

        if (normalized.Length > MaxLength)
        {
            throw new DomainException($"{fieldName} must not exceed {MaxLength} characters.");
        }

        if (!NameRegex().IsMatch(normalized))
        {
            throw new DomainException($"{fieldName} contains invalid characters.");
        }

        return new PersonName(normalized);
    }

    /// <summary>
    /// Creates an optional name component (e.g., MiddleName). Returns null for empty input.
    /// </summary>
    public static PersonName? CreateOptional(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return Create(value, fieldName);
    }

    private static string Normalize(string value)
    {
        var trimmed = value.Trim();
        return CollapseWhitespaceRegex().Replace(trimmed, " ");
    }

    [GeneratedRegex(@"^[\p{IsArabic}a-zA-Z\s'\-]+$")]
    private static partial Regex NameRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex CollapseWhitespaceRegex();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
