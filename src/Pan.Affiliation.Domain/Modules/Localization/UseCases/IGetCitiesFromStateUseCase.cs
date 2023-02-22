using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Shared.UseCases;

namespace Pan.Affiliation.Domain.Modules.Localization.UseCases
{
    public interface IGetCitiesFromStateUseCase : IUseCase<int, IEnumerable<City>?>
    {
    }
}
