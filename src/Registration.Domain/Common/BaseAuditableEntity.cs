namespace Registration.Domain.Common;

/// <summary>
/// Base class for entities that require audit tracking (created/updated metadata).
/// </summary>
public abstract class BaseAuditableEntity : BaseEntity
{
    public DateTime CreatedAtUtc { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public string? UpdatedBy { get; set; }
}
