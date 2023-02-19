using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Domain.Localization.Adapters
{
    public interface IStateAdapter
    {
        Task<IEnumerable<State>?> GetStatesAsync();
    }
}
