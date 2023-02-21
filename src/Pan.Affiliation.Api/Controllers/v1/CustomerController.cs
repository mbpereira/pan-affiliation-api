using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Api.Contracts;
using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.UseCases;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Api.Controllers.v1;

public class CustomerController : DefaultController
{
    public CustomerController(IValidationContext context) : base(context)
    {
    }

    [HttpGet("{documentNumber}")]
    public async Task<ActionResult<GenericResponse<Customer?>>>
        GetCustomerByDocumentNumberAsync(
            string documentNumber,
            [FromServices] IGetCustomerByDocumentNumberUseCase useCase)
        => GenericResponse(await useCase.ExecuteAsync(documentNumber));
}