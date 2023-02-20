using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Application.UseCases.GetStates
{
    public interface IGetCountryStatesUseCase : IParameterlessUseCase<IEnumerable<State>?>
    {
    }
}
