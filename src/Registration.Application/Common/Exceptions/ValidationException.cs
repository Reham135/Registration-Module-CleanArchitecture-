using FluentValidation.Results;

namespace Registration.Application.Common.Exceptions;

/// <summary>
/// Thrown by <see cref="Behaviours.ValidationBehaviour{TRequest,TResponse}"/> when one or more
/// FluentValidation validators fail. Mapped to HTTP 400 (ValidationProblemDetails) by the API layer.
/// </summary>
public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy(f => f.PropertyName, f => f.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }

    public IDictionary<string, string[]> Errors { get; }
}
