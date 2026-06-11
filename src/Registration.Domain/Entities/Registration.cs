using Registration.Domain.Common;
using Registration.Domain.Exceptions;
using Registration.Domain.ValueObjects;

namespace Registration.Domain.Entities;

/// <summary>
/// Aggregate root representing a registered person and their addresses.
/// </summary>
public class Registration : BaseAuditableEntity
{
    public const int MinAge = 20;
    public const int MinAddresses = 1;
    public const int MaxAddresses = 5;

    private readonly List<Address> _addresses = new();

    public PersonName FirstName { get; private set; } = null!;

    public PersonName? MiddleName { get; private set; }

    public PersonName LastName { get; private set; } = null!;

    public DateOnly BirthDate { get; private set; }

    public MobileNumber MobileNumber { get; private set; } = null!;

    public Email Email { get; private set; } = null!;

    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    /// <summary>
    /// Parameterless constructor required by EF Core.
    /// </summary>
    protected Registration()
    {
    }

    private Registration(
        PersonName firstName,
        PersonName? middleName,
        PersonName lastName,
        DateOnly birthDate,
        MobileNumber mobileNumber,
        Email email)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        BirthDate = birthDate;
        MobileNumber = mobileNumber;
        Email = email;
    }

    /// <summary>
    /// Factory method enforcing the registration invariants:
    /// - birth date must not be in the future
    /// - the registrant must be at least <see cref="MinAge"/> years old
    /// - between <see cref="MinAddresses"/> and <see cref="MaxAddresses"/> addresses (inclusive)
    /// - exactly one address must be marked as primary (auto-promoted when only one address is supplied)
    /// </summary>
    public static Registration Create(
        string? firstName,
        string? middleName,
        string? lastName,
        DateOnly birthDate,
        string? mobileNumber,
        string? email,
        IReadOnlyCollection<Address> addresses,
        DateOnly today)
    {
        if (birthDate > today)
        {
            throw new DomainException("Birth date cannot be in the future.");
        }

        var age = AgeCalculator.CalculateAge(birthDate, today);
        if (age < MinAge)
        {
            throw new DomainException($"Registrant must be at least {MinAge} years old.");
        }

        var registration = new Registration(
            PersonName.Create(firstName, nameof(FirstName)),
            PersonName.CreateOptional(middleName, nameof(MiddleName)),
            PersonName.Create(lastName, nameof(LastName)),
            birthDate,
            MobileNumber.Create(mobileNumber),
            Email.Create(email));

        registration.SetAddresses(addresses);

        return registration;
    }

    private void SetAddresses(IReadOnlyCollection<Address> addresses)
    {
        if (addresses.Count is < MinAddresses or > MaxAddresses)
        {
            throw new InvalidAddressConfigurationException(
                $"A registration must have between {MinAddresses} and {MaxAddresses} addresses.");
        }

        var primaryCount = addresses.Count(a => a.IsPrimary);

        if (addresses.Count == 1)
        {
            // Auto-promote the single address to primary regardless of the supplied flag.
            addresses.First().SetAsPrimary(true);
        }
        else if (primaryCount != 1)
        {
            throw new InvalidAddressConfigurationException(
                "Exactly one address must be marked as primary when multiple addresses are provided.");
        }

        foreach (var address in addresses)
        {
            address.AssignToRegistration(Id);
            _addresses.Add(address);
        }
    }
}
