using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Application.UseCases.GetCitiesFromState;
using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Api.Controllers.v1
{
    public class CitiesController : DefaultController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>?>> GetCitiesFromStateAsync(int stateId, [FromServices] IGetCitiesFromStateUseCase useCase)
        {
            return Ok(await useCase.ExecuteAsync(stateId));
        }
    }
}
