namespace Registration.Domain.Common;

/// <summary>
/// Base class for all entities with an integer surrogate key.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; protected set; }
}
