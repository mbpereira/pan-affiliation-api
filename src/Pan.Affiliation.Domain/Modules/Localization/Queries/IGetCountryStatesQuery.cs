using Pan.Affiliation.Domain.Modules.Localization.Entities;

namespace Pan.Affiliation.Domain.Modules.Localization.Queries
{
    public interface IGetCountryStatesQuery
    {
        Task<IEnumerable<State>?> GetCountryStatesAsync();
    }
}
