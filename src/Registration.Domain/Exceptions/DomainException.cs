namespace Registration.Domain.Exceptions;

/// <summary>
/// Base exception for domain invariant violations. Mapped to HTTP 400 by the API layer.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}
