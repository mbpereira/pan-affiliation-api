using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Application.UseCases.GetCountryStates
{
    public interface IGetCountryStatesUseCase : IParameterlessUseCase<IEnumerable<State>?>
    {
    }
}
