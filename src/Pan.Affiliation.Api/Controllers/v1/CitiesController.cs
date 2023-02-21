using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Api.Contracts;
using Pan.Affiliation.Application.UseCases.GetCitiesFromState;
using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.Shared;

namespace Pan.Affiliation.Api.Controllers.v1
{
    public class CitiesController : DefaultController
    {
        public CitiesController(IValidationContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<City>?>>> GetCitiesFromStateAsync(int stateId,
            [FromServices] IGetCitiesFromStateUseCase useCase)
            => GenericResponse(await useCase.ExecuteAsync(stateId));
    }
}