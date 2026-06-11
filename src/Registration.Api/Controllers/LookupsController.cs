using MediatR;
using Microsoft.AspNetCore.Mvc;
using Registration.Api.SwaggerExamples;
using Registration.Application.Lookups.DTOs;
using Registration.Application.Lookups.Queries.GetCitiesByGovernorate;
using Registration.Application.Lookups.Queries.GetGovernorates;
using Swashbuckle.AspNetCore.Filters;

namespace Registration.Api.Controllers;

[ApiController]
[Route("api/lookups")]
[Produces("application/json")]
public class LookupsController : ControllerBase
{
    private readonly ISender _sender;

    public LookupsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Returns all active governorates.
    /// </summary>
    /// <response code="200">The list of active governorates.</response>
    [HttpGet("governorates")]
    [ProducesResponseType(typeof(List<GovernorateDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GovernorateListExample))]
    public async Task<ActionResult<List<GovernorateDto>>> GetGovernorates(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetGovernoratesQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Returns all active cities belonging to the given governorate.
    /// </summary>
    /// <param name="governorateId">The id of the governorate to filter cities by.</param>
    /// <response code="200">The list of active cities for the governorate (empty if the governorate does not exist).</response>
    [HttpGet("cities")]
    [ProducesResponseType(typeof(List<CityDto>), StatusCodes.Status200OK)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CityListExample))]
    public async Task<ActionResult<List<CityDto>>> GetCitiesByGovernorate([FromQuery] int governorateId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCitiesByGovernorateQuery(governorateId), cancellationToken);
        return Ok(result);
    }
}
