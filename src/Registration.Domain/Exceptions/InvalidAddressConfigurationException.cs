namespace Registration.Domain.Exceptions;

/// <summary>
/// Raised when the set of addresses for a registration violates count or
/// primary-address invariants.
/// </summary>
public class InvalidAddressConfigurationException : DomainException
{
    public InvalidAddressConfigurationException(string message) : base(message)
    {
    }
}
