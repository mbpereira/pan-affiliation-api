using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Application.UseCases.GetPostalCodeInformation;
using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Api.Controllers.v1;

public class PostalCodeController : DefaultController
{
    [HttpGet]
    public async Task<ActionResult<PostalCodeInformation>> GetInformationAsync(
        string postalCode,
        [FromServices] IGetPostalCodeInformationUseCase service) =>
            Ok(await service.ExecuteAsync(postalCode));
}