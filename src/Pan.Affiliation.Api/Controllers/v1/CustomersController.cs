using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Api.Contracts;
using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.UseCases.ChangeAddress;
using Pan.Affiliation.Domain.Modules.Customers.UseCases.GetCustomerByDocumentNumber;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Api.Controllers.v1;

public class CustomersController : DefaultController
{
    public CustomersController(IValidationContext context) : base(context)
    {
    }

    [HttpGet("{documentNumber}")]
    public async Task<ActionResult<GenericResponse<Customer?>>>
        GetCustomerByDocumentNumberAsync(
            string documentNumber,
            [FromServices] IGetCustomerByDocumentNumberUseCase useCase)
        => GenericResponse(await useCase.ExecuteAsync(documentNumber));
    
    [HttpPut("{customerId}/addresses/{addressId}")]
    public async Task<ActionResult<GenericResponse<Address?>>>
        PutAddressAsync(
            [FromRoute] Guid customerId,
            [FromRoute] Guid addressId,
            [FromBody] AddressInput input,
            [FromServices] IChangeAddressUseCase useCase)
        => GenericResponse(await useCase.ExecuteAsync(new(customerId, addressId, input)));

}