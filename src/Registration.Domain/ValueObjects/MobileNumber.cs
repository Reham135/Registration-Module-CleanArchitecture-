using System.Text.RegularExpressions;
using Registration.Domain.Exceptions;

namespace Registration.Domain.ValueObjects;

/// <summary>
/// Represents a mobile phone number, normalized and validated against the E.164 format.
/// </summary>
public sealed partial class MobileNumber : Common.ValueObject
{
    public string Value { get; }

    private MobileNumber(string value)
    {
        Value = value;
    }

    public static MobileNumber Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Mobile number is required.");
        }

        var normalized = Normalize(value);

        if (!E164Regex().IsMatch(normalized))
        {
            throw new DomainException("Mobile number must be a valid E.164 format (e.g. +201006158123).");
        }

        return new MobileNumber(normalized);
    }

    /// <summary>
    /// Strips spaces, dashes, and parentheses from the input.
    /// </summary>
    public static string Normalize(string value)
    {
        return RemoveFormattingRegex().Replace(value.Trim(), string.Empty);
    }

    [GeneratedRegex(@"[\s\-\(\)]")]
    private static partial Regex RemoveFormattingRegex();

    [GeneratedRegex(@"^\+[1-9]\d{6,14}$")]
    private static partial Regex E164Regex();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
