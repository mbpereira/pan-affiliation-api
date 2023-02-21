using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Application.UseCases.GetCitiesFromState;
using Pan.Affiliation.Domain.Shared;

namespace Pan.Affiliation.Api.Controllers.v1
{
    public class CitiesController : DefaultController
    {
        public CitiesController(IValidationContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetCitiesFromStateAsync(int stateId,
            [FromServices] IGetCitiesFromStateUseCase useCase)
            => GenericResponse(await useCase.ExecuteAsync(stateId));
    }
}