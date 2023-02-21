using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Application.UseCases.GetStates;
using Pan.Affiliation.Domain.Shared;

namespace Pan.Affiliation.Api.Controllers.v1
{
    public class StatesController : DefaultController
    {
        public StatesController(IValidationContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetCountryStatesAsync(
            [FromServices] IGetCountryStatesUseCase useCase)
            => GenericResponse(await useCase.ExecuteAsync());
    }
}