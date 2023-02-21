using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Shared.UseCase;

namespace Pan.Affiliation.Application.UseCases.GetCitiesFromState
{
    public interface IGetCitiesFromStateUseCase : IUseCase<int, IEnumerable<City>?>
    {
    }
}
