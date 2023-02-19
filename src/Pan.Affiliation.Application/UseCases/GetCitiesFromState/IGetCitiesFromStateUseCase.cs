using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Application.UseCases.GetCitiesFromState
{
    public interface IGetCitiesFromStateUseCase : IUseCase<int, IEnumerable<City>?>
    {
    }
}
