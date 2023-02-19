using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Domain.Localization.Adapters
{
    public interface ICountryStatesService
    {
        Task<IEnumerable<State>?> GetCountryStatesAsync();
    }
}
