using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.Logging;

namespace Pan.Affiliation.Application.UseCases.GetCitiesFromState
{
    public class GetCitiesFromStateUseCase : IGetCitiesFromStateUseCase
    {
        private readonly ICityService _cityService;
        private readonly ILogger<GetCitiesFromStateUseCase> _logger;

        public GetCitiesFromStateUseCase(ICityService cityService, ILogger<GetCitiesFromStateUseCase> logger)
        {
            _cityService = cityService;
            _logger = logger;
        }

        public Task<IEnumerable<City>?> ExecuteAsync(int param)
        {
            _logger.LogInformation("Getting cities from state identified by {stateId}", param);
            return _cityService.GetCitiesFromStateAsync(stateId: param);
        }
    }
}
