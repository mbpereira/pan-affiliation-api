using System.Net;
using AutoBogus;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Api.Contracts;
using Pan.Affiliation.Domain.Shared;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Api.Contracts;

public class GenericResponseFactoryTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void When_ValidationStatus_is_not_defined_and_has_errors_should_return_bad_request_with_errors()
    {
        var error = new AutoFaker<Error>()
            .Generate();
        var context = new ValidationContext();
        context.AddNotification(error);

        var actionResult =
            new GenericResponseFactory<object>(null, context).Create();
        var objectResult = actionResult.Result;

        objectResult.Should().BeEquivalentTo(new
        {
            StatusCode = HttpStatusCode.BadRequest.GetHashCode(),
            Value = new GenericResponse<object>()
            {
                Data = null,
                Errors = context.Errors
            }
        });
    }

    [Fact]
    public void When_ValidationStatus_is_not_defined_and_context_does_not_have_errors_should_return_success()
    {
        var context = new ValidationContext();
        var data = new
        {
            Name = _faker.Person.FirstName
        };

        var actionResult =
            new GenericResponseFactory<object>(data, context).Create();
        var objectResult = actionResult.Result;

        objectResult.Should().BeEquivalentTo(new
        {
            StatusCode = HttpStatusCode.OK.GetHashCode(),
            Value = new GenericResponse<object>()
            {
                Data = data
            }
        });
    }

    [Fact]
    public void
        When_ValidationStatus_is_not_defined_and_context_does_not_have_errors_should_return_no_content_if_data_is_null()
    {
        var context = new ValidationContext();

        var actionResult =
            new GenericResponseFactory<object>(null, context).Create();
        var objectResult = actionResult.Result;

        objectResult.Should().BeEquivalentTo(new
        {
            StatusCode = HttpStatusCode.NoContent.GetHashCode()
        });
    }

    [Fact]
    public void When_ValidationStatus_is_set_to_failed_should_return_bad_request()
    {
        var error = new AutoFaker<Error>()
            .Generate();
        var context = new ValidationContext();
        context.AddNotification(error);
        context.SetStatus(ValidationStatus.Failed);

        var actionResult =
            new GenericResponseFactory<object>(null, context).Create();
        var objectResult = actionResult.Result;

        objectResult.Should().BeEquivalentTo(new
        {
            StatusCode = HttpStatusCode.BadRequest.GetHashCode(),
            Value = new GenericResponse<object>()
            {
                Data = null,
                Errors = context.Errors
            }
        });
    }
    
    [Fact]
    public void When_ValidationStatus_is_set_to_partial_success_should_return_partial_content()
    {
        var error = new AutoFaker<Error>()
            .Generate();
        var context = new ValidationContext();
        context.AddNotification(error);
        context.SetStatus(ValidationStatus.PartialSuccess);

        var actionResult =
            new GenericResponseFactory<object>(null, context).Create();
        var objectResult = actionResult.Result;

        objectResult.Should().BeEquivalentTo(new
        {
            StatusCode = HttpStatusCode.PartialContent.GetHashCode(),
            Value = new GenericResponse<object>()
            {
                Data = null,
                Errors = context.Errors
            }
        });
    }
    
    [Fact]
    public void When_ValidationStatus_is_set_to_success_should_return_success_if_data_is_not_null()
    {
        var context = new ValidationContext();
        context.SetStatus(ValidationStatus.Success);
        var data = new
        {
            Name = _faker.Person.FirstName
        };
        
        var actionResult =
            new GenericResponseFactory<object>(data, context).Create();
        var objectResult = actionResult.Result;

        objectResult.Should().BeEquivalentTo(new
        {
            StatusCode = HttpStatusCode.OK.GetHashCode(),
            Value = new GenericResponse<object>()
            {
                Data = data,
                Errors = context.Errors
            }
        });
    }
    
    [Fact]
    public void When_ValidationStatus_is_set_to_not_found_should_return_not_found_status_code()
    {
        var context = new ValidationContext();
        context.SetStatus(ValidationStatus.NotFound);
        
        var actionResult =
            new GenericResponseFactory<object>(null, context).Create();
        var objectResult = actionResult.Result;

        objectResult.Should().BeEquivalentTo(new
        {
            StatusCode = HttpStatusCode.NotFound.GetHashCode(),
        });
    }
    
    [Fact]
    public void When_ValidationStatus_is_set_to_success_but_data_is_null_should_return_no_content_status_code()
    {
        var context = new ValidationContext();
        context.SetStatus(ValidationStatus.Success);
        
        var actionResult =
            new GenericResponseFactory<object>(null, context).Create();
        var objectResult = actionResult.Result;

        objectResult.Should().BeEquivalentTo(new
        {
            StatusCode = HttpStatusCode.NoContent.GetHashCode(),
        });
    }
}