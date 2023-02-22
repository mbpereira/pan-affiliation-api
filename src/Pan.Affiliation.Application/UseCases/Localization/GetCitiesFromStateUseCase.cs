using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Modules.Localization.Queries;
using Pan.Affiliation.Domain.Modules.Localization.UseCases;
using Pan.Affiliation.Domain.Shared.Logging;

namespace Pan.Affiliation.Application.UseCases.Localization
{
    public class GetCitiesFromStateUseCase : IGetCitiesFromStateUseCase
    {
        private readonly IGetCityFromStateQuery _getCityFromStateQuery;
        private readonly ILogger<GetCitiesFromStateUseCase> _logger;

        public GetCitiesFromStateUseCase(IGetCityFromStateQuery getCityFromStateQuery, ILogger<GetCitiesFromStateUseCase> logger)
        {
            _getCityFromStateQuery = getCityFromStateQuery;
            _logger = logger;
        }

        public Task<IEnumerable<City>?> ExecuteAsync(int param)
        {
            _logger.LogInformation("Getting cities from state identified by {stateId}", param);
            return _getCityFromStateQuery.GetCitiesFromStateAsync(stateId: param);
        }
    }
}
