using System.Net;
using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Api.Contracts;

public class GenericResponseFactory<T> where T : class
{
    private readonly T? _data;
    private readonly IValidationContext _context;

    public GenericResponseFactory(T? data, IValidationContext context)
    {
        _data = data;
        _context = context;
    }

    public ActionResult<GenericResponse<T?>> Create()
    {
        if (_context is { ValidationStatus: null })
        {
            if (_context is { HasErrors: true })
                return CreateGenericResponse(HttpStatusCode.BadRequest, _data);

            if (_data is not null)
                return CreateGenericResponse(HttpStatusCode.OK, _data);

            return StatusCodeResult(HttpStatusCode.NoContent);
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

    private ActionResult StatusCodeResult(HttpStatusCode statusCode) =>
        new StatusCodeResult(statusCode.GetHashCode());

    private ActionResult<GenericResponse<T?>> CreateGenericResponse(HttpStatusCode statusCode, T? data = null)
    {
        if (data is null && !_context.HasErrors)
            return StatusCodeResult(statusCode);

        var response = new GenericResponse<T>()
        {
            Data = data,
            Errors = _context.Errors
        };

        return new ObjectResult(response)
        {
            StatusCode = statusCode.GetHashCode()
        };
    }
}