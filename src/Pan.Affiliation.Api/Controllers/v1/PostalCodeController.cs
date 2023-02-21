using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Api.Contracts;
using Pan.Affiliation.Application.UseCases.GetPostalCodeInformation;
using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.Shared;

namespace Pan.Affiliation.Api.Controllers.v1;

public class PostalCodeController : DefaultController
{
    public PostalCodeController(ValidationContext context) : base(context)
    {
    }
    
    [HttpGet]
    public async Task<ActionResult<GenericResponse<PostalCodeInformation?>>> GetInformationAsync(
        string postalCode,
        [FromServices] IGetPostalCodeInformationUseCase service) =>
            GenericResponse(await service.ExecuteAsync(postalCode));
}