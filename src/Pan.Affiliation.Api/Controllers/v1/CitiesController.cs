using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Api.Contracts;
using Pan.Affiliation.Application.UseCases.GetCitiesFromState;
using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Shared;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Api.Controllers.v1
{
    public class CitiesController : DefaultController
    {
        public CitiesController(IValidationContext context) : base(context)
        {
        }

        [HttpGet("{stateId}")]
        public async Task<ActionResult<GenericResponse<IEnumerable<City>?>>> GetCitiesFromStateAsync(int stateId,
            [FromServices] IGetCitiesFromStateUseCase useCase)
            => GenericResponse(await useCase.ExecuteAsync(stateId));
    }
}