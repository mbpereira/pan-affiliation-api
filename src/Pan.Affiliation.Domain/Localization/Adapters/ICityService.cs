using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Domain.Localization.Adapters
{
    public interface ICityService
    {
        Task<IEnumerable<City>?> GetCitiesFromStateAsync(int stateId);
    }
}
