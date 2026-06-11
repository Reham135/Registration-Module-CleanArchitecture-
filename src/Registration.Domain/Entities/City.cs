using Registration.Domain.Common;

namespace Registration.Domain.Entities;

/// <summary>
/// Represents a city belonging to a governorate, used as an address lookup.
/// </summary>
public class City : BaseEntity
{
    public string Name { get; internal set; } = null!;

    public int GovernorateId { get; internal set; }

    public Governorate? Governorate { get; internal set; }

    public bool IsActive { get; internal set; }

    /// <summary>
    /// Parameterless constructor required by EF Core.
    /// </summary>
    protected City()
    {
    }

    public City(int id, string name, int governorateId, bool isActive = true)
    {
        Id = id;
        Name = name;
        GovernorateId = governorateId;
        IsActive = isActive;
    }
}
