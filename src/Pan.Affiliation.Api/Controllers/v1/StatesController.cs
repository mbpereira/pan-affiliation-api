using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Application.UseCases.GetStates;
using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Api.Controllers.v1
{
    public class StatesController : DefaultController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetCountryStatesAsync([FromServices] IGetStatesUseCase useCase)
        {
            return Ok(await useCase.ExecuteAsync());
        }
    }
}
