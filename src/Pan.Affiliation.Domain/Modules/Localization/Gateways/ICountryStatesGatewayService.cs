using Pan.Affiliation.Domain.Modules.Localization.Entities;

namespace Pan.Affiliation.Domain.Modules.Localization.Gateways
{
    public interface ICountryStatesGatewayService
    {
        Task<IEnumerable<State>?> GetCountryStatesAsync();
    }
}
