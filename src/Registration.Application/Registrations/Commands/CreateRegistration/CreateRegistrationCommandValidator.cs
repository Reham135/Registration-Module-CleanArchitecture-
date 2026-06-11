using System.Text.RegularExpressions;
using FluentValidation;
using Registration.Application.Common.Interfaces;
using Registration.Domain.Common;
using Registration.Domain.Entities;

namespace Registration.Application.Registrations.Commands.CreateRegistration;

/// <summary>
/// Validates <see cref="CreateRegistrationCommand"/>. Combines synchronous format/length
/// rules with asynchronous business rules (age, governorate/city existence) that require
/// access to <see cref="IDateTimeProvider"/> and <see cref="ILookupRepository"/>.
/// </summary>
public partial class CreateRegistrationCommandValidator : AbstractValidator<CreateRegistrationCommand>
{
    private static readonly Regex NameRegex = MyNameRegex();
    private static readonly Regex BuildingFlatRegex = MyBuildingFlatRegex();

    private readonly ILookupRepository _lookupRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateRegistrationCommandValidator(ILookupRepository lookupRepository, IDateTimeProvider dateTimeProvider)
    {
        _lookupRepository = lookupRepository;
        _dateTimeProvider = dateTimeProvider;

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.")
            .Matches(NameRegex).WithMessage("First name contains invalid characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
            .Matches(NameRegex).WithMessage("Last name contains invalid characters.");

        RuleFor(x => x.MiddleName)
            .MaximumLength(50).WithMessage("Middle name must not exceed 50 characters.")
            .Matches(NameRegex).WithMessage("Middle name contains invalid characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.MiddleName));

        RuleFor(x => x.BirthDate)
            .Must(birthDate => birthDate <= _dateTimeProvider.Today)
            .WithMessage("Birth date cannot be in the future.")
            .Must(birthDate => AgeCalculator.CalculateAge(birthDate, _dateTimeProvider.Today) >= Domain.Entities.Registration.MinAge)
            .WithMessage($"Registrant must be at least {Domain.Entities.Registration.MinAge} years old.");

        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile number is required.")
            .Must(mobile => MobileNumberRegex().IsMatch(Domain.ValueObjects.MobileNumber.Normalize(mobile ?? string.Empty)))
            .WithMessage("Mobile number must be a valid E.164 format (e.g. +201006158123).");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(254).WithMessage("Email must not exceed 254 characters.")
            .EmailAddress().WithMessage("Email format is invalid.");

        RuleFor(x => x.Addresses)
            .NotNull().WithMessage("At least one address is required.")
            .Must(a => a.Count is >= Domain.Entities.Registration.MinAddresses and <= Domain.Entities.Registration.MaxAddresses)
            .WithMessage($"A registration must have between {Domain.Entities.Registration.MinAddresses} and {Domain.Entities.Registration.MaxAddresses} addresses.");

        RuleFor(x => x.Addresses)
            .Must(addresses => addresses.Count == 1 || addresses.Count(a => a.IsPrimary) == 1)
            .WithMessage("Exactly one address must be marked as primary when multiple addresses are provided.")
            .When(x => x.Addresses is { Count: >= 1 });

        RuleForEach(x => x.Addresses).ChildRules(address =>
        {
            address.RuleFor(a => a.Street)
                .NotEmpty().WithMessage("Street is required.")
                .MaximumLength(Address.StreetMaxLength).WithMessage($"Street must not exceed {Address.StreetMaxLength} characters.");

            address.RuleFor(a => a.BuildingNumber)
                .NotEmpty().WithMessage("Building number is required.")
                .MaximumLength(Address.BuildingFlatMaxLength).WithMessage($"Building number must not exceed {Address.BuildingFlatMaxLength} characters.")
                .Matches(BuildingFlatRegex).WithMessage("Building number contains invalid characters.")
                .When(a => !string.IsNullOrWhiteSpace(a.BuildingNumber), ApplyConditionTo.CurrentValidator);

            address.RuleFor(a => a.FlatNumber)
                .NotEmpty().WithMessage("Flat number is required.")
                .MaximumLength(Address.BuildingFlatMaxLength).WithMessage($"Flat number must not exceed {Address.BuildingFlatMaxLength} characters.")
                .Matches(BuildingFlatRegex).WithMessage("Flat number contains invalid characters.")
                .When(a => !string.IsNullOrWhiteSpace(a.FlatNumber), ApplyConditionTo.CurrentValidator);

            address.RuleFor(a => a.GovernorateId)
                .MustAsync((governorateId, cancellationToken) => _lookupRepository.GovernorateExistsAsync(governorateId, cancellationToken))
                .WithMessage(a => $"Governorate with id {a.GovernorateId} does not exist or is inactive.");

            address.RuleFor(a => a.CityId)
                .MustAsync((address, cityId, cancellationToken) => _lookupRepository.CityBelongsToGovernorateAsync(cityId, address.GovernorateId, cancellationToken))
                .WithMessage(a => $"City with id {a.CityId} does not belong to governorate {a.GovernorateId} or is inactive.");
        });
    }

    [GeneratedRegex(@"^[\p{IsArabic}a-zA-Z\s'\-]+$")]
    private static partial Regex MyNameRegex();

    [GeneratedRegex(@"^[A-Za-z0-9؀-ۿ\/\-\s]+$")]
    private static partial Regex MyBuildingFlatRegex();

    [GeneratedRegex(@"^\+[1-9]\d{6,14}$")]
    private static partial Regex MobileNumberRegex();
}
