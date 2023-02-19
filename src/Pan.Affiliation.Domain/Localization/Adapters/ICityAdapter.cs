using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Domain.Localization.Adapters
{
    public interface ICityAdapter
    {
        Task<IEnumerable<City>?> GetCitiesFromState(int stateId);
    }
}
