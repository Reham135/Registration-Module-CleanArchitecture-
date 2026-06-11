using MediatR;
using Microsoft.AspNetCore.Mvc;
using Registration.Api.SwaggerExamples;
using Registration.Application.Registrations.Commands.CreateRegistration;
using Registration.Application.Registrations.DTOs;
using Registration.Application.Registrations.Queries.GetRegistrationById;
using Swashbuckle.AspNetCore.Filters;

namespace Registration.Api.Controllers;

[ApiController]
[Route("api/registrations")]
[Produces("application/json")]
public class RegistrationsController : ControllerBase
{
    private readonly ISender _sender;

    public RegistrationsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Registers a new person along with 1-5 addresses.
    /// </summary>
    /// <response code="201">The registration was created successfully.</response>
    /// <response code="400">The request failed validation or violated a business rule.</response>
    /// <response code="409">A registration with the same email or mobile number already exists.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateRegistrationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [SwaggerRequestExample(typeof(CreateRegistrationCommand), typeof(CreateRegistrationCommandExample))]
    [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CreateRegistrationResponseExample))]
    public async Task<IActionResult> Create([FromBody] CreateRegistrationCommand command, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, new CreateRegistrationResponse(id));
    }

    /// <summary>
    /// Retrieves a registration by id, including its addresses and address lookup names.
    /// </summary>
    /// <response code="200">The registration was found.</response>
    /// <response code="404">No registration exists with the given id.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(RegistrationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RegistrationDtoExample))]
    public async Task<ActionResult<RegistrationDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetRegistrationByIdQuery(id), cancellationToken);
        return Ok(result);
    }
}

/// <summary>
/// Response returned after successfully creating a registration.
/// </summary>
public record CreateRegistrationResponse(int Id);
