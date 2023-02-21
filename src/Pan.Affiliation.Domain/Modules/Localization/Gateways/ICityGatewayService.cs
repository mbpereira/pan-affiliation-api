using Pan.Affiliation.Domain.Modules.Localization.Entities;

namespace Pan.Affiliation.Domain.Modules.Localization.Gateways
{
    public interface ICityGatewayService
    {
        Task<IEnumerable<City>?> GetCitiesFromStateAsync(int stateId);
    }
}
