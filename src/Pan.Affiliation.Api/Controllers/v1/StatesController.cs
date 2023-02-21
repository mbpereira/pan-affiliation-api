using Microsoft.AspNetCore.Mvc;
using Pan.Affiliation.Api.Contracts;
using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Modules.Localization.UseCases;
using Pan.Affiliation.Domain.Shared;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Api.Controllers.v1
{
    public class StatesController : DefaultController
    {
        public StatesController(IValidationContext context) : base(context)
        {
        }

        [HttpGet]
        public async Task<ActionResult<GenericResponse<IEnumerable<State>?>>> GetCountryStatesAsync(
            [FromServices] IGetCountryStatesUseCase useCase)
            => GenericResponse(await useCase.ExecuteAsync());
    }
}