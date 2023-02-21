using Pan.Affiliation.Domain.Modules.Localization.Entities;

namespace Pan.Affiliation.Domain.Modules.Localization.Queries
{
    public interface IGetCityFromStateQuery
    {
        Task<IEnumerable<City>?> GetCitiesFromStateAsync(int stateId);
    }
}
