using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Api.Contracts;
using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Modules.Localization.UseCases;
using Pan.Affiliation.Domain.Shared;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Api.Controllers.v1;

public class PostalCodeController : DefaultController
{
    public PostalCodeController(IValidationContext context) : base(context)
    {
    }
    
    [HttpGet("{postalCode}")]
    public async Task<ActionResult<GenericResponse<PostalCodeInformation?>>> GetInformationAsync(
        string postalCode,
        [FromServices] IGetPostalCodeInformationUseCase service) =>
            GenericResponse(await service.ExecuteAsync(postalCode));
}