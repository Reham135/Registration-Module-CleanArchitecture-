using Registration.Domain.Common;

namespace Registration.Domain.Entities;

/// <summary>
/// Represents a governorate (top-level administrative division) used as an address lookup.
/// </summary>
public class Governorate : BaseEntity
{
    private readonly List<City> _cities = new();

    public string Name { get; internal set; } = null!;

    public bool IsActive { get; internal set; }

    public IReadOnlyCollection<City> Cities => _cities.AsReadOnly();

    /// <summary>
    /// Parameterless constructor required by EF Core.
    /// </summary>
    protected Governorate()
    {
    }

    public Governorate(int id, string name, bool isActive = true)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
    }
}
