using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Shared.UseCase;

namespace Pan.Affiliation.Application.UseCases.GetCountryStates
{
    public interface IGetCountryStatesUseCase : IParameterlessUseCase<IEnumerable<State>?>
    {
    }
}
