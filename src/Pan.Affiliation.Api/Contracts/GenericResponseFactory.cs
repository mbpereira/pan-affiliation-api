using System.Net;
using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Domain.Shared;

namespace Pan.Affiliation.Api.Contracts;

public class GenericResponseFactory
{
    private readonly object? _data;
    private readonly IValidationContext _context;

    public GenericResponseFactory(object? data, IValidationContext context)
    {
        _data = data;
        _context = context;
    }
    
    public IActionResult Create()
    {
        if (_context is { ValidationStatus: null })
        {
            if (_context is { HasErrors: true })
                return CreateGenericResponse(HttpStatusCode.BadRequest, _data);

            if (_data is not null)
                return CreateGenericResponse(HttpStatusCode.OK, _data);

            return StatusCodeResult(HttpStatusCode.NotFound);
        }

        if (_context is { ValidationStatus: ValidationStatus.Failed })
            return CreateGenericResponse(HttpStatusCode.BadRequest, _data);

        if (_context is { ValidationStatus: ValidationStatus.PartialSuccess })
            return CreateGenericResponse(HttpStatusCode.PartialContent, _data);

        if (_context is { ValidationStatus: ValidationStatus.Success } && _data is not null)
            return CreateGenericResponse(HttpStatusCode.OK, _data);

        if (_context is { ValidationStatus: ValidationStatus.NotFound })
            return StatusCodeResult(HttpStatusCode.NotFound);

        return StatusCodeResult(HttpStatusCode.NoContent);
    }

    private IActionResult StatusCodeResult(HttpStatusCode statusCode) =>
        new StatusCodeResult(statusCode.GetHashCode());
    
    private IActionResult CreateGenericResponse(HttpStatusCode statusCode, object? data = null)
    {
        if (data is null && !_context.HasErrors)
            return StatusCodeResult(statusCode);

        var response = new
        {
            data,
            errors = _context.Errors
        };

        return new ObjectResult(response)
        {
            StatusCode = statusCode.GetHashCode()
        };
    }
}