using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Registration.Application.Common.Exceptions;
using Registration.Domain.Exceptions;
using ValidationException = Registration.Application.Common.Exceptions.ValidationException;

namespace Registration.Api.Middleware;

/// <summary>
/// Translates exceptions thrown anywhere in the request pipeline into a consistent
/// ProblemDetails (or ValidationProblemDetails) response.
/// Registered first in the pipeline so it can catch exceptions from all downstream middleware.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var (statusCode, problemDetails) = exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                (object)new ValidationProblemDetails(validationException.Errors)
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "One or more validation errors occurred.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Instance = context.Request.Path,
                }),

            NotFoundException notFoundException => (
                HttpStatusCode.NotFound,
                CreateProblemDetails(context, HttpStatusCode.NotFound, "Resource not found", notFoundException.Message)),

            ConflictException conflictException => (
                HttpStatusCode.Conflict,
                CreateProblemDetails(context, HttpStatusCode.Conflict, "Conflict", conflictException.Message)),

            DomainException domainException => (
                HttpStatusCode.BadRequest,
                CreateProblemDetails(context, HttpStatusCode.BadRequest, "Business rule violation", domainException.Message)),

            _ => (
                HttpStatusCode.InternalServerError,
                CreateProblemDetails(context, HttpStatusCode.InternalServerError, "An unexpected error occurred", "An unexpected error occurred. Please try again later.")),
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "Unhandled exception while processing {Method} {Path}", context.Request.Method, context.Request.Path);
        }
        else
        {
            _logger.LogWarning(exception, "Handled exception ({StatusCode}) while processing {Method} {Path}", (int)statusCode, context.Request.Method, context.Request.Path);
        }

        context.Response.StatusCode = (int)statusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        }));
    }

    private static ProblemDetails CreateProblemDetails(HttpContext context, HttpStatusCode statusCode, string title, string detail)
    {
        return new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Type = $"https://tools.ietf.org/html/rfc7231#section-6.{(int)statusCode / 100}.{(int)statusCode % 100}",
            Instance = context.Request.Path,
        };
    }
}
