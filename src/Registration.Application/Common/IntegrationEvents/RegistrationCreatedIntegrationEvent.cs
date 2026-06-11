namespace Registration.Application.Common.IntegrationEvents;

/// <summary>
/// Published after a registration has been successfully persisted, so that
/// out-of-process consumers (welcome email/SMS, audit log, external systems)
/// can react asynchronously without affecting the create-registration transaction.
/// </summary>
public record RegistrationCreatedIntegrationEvent(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string MobileNumber,
    DateTime CreatedAtUtc);
