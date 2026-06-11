namespace Registration.Application.Lookups.DTOs;

public class CityDto
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int GovernorateId { get; set; }
}
