namespace Registration.Domain.Common;

/// <summary>
/// Shared age calculation logic used by both the domain entity invariant
/// and the application-layer validator, to avoid duplication/drift.
/// </summary>
public static class AgeCalculator
{
    /// <summary>
    /// Calculates the age in completed years as of the given reference date.
    /// </summary>
    public static int CalculateAge(DateOnly birthDate, DateOnly asOf)
    {
        var age = asOf.Year - birthDate.Year;

        if (asOf < birthDate.AddYears(age))
        {
            age--;
        }

        return age;
    }
}
