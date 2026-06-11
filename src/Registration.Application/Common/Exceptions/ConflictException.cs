namespace Registration.Application.Common.Exceptions;

/// <summary>
/// Thrown when a request conflicts with existing state (e.g., duplicate email/mobile).
/// Mapped to HTTP 409 by the API layer.
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}
