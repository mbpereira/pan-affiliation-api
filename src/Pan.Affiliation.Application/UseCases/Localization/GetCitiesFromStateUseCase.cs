using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Modules.Localization.Gateways;
using Pan.Affiliation.Domain.Modules.Localization.UseCases;
using Pan.Affiliation.Domain.Shared.Logging;

namespace Pan.Affiliation.Application.UseCases.Localization
{
    public class GetCitiesFromStateUseCase : IGetCitiesFromStateUseCase
    {
        private readonly ICityGatewayService _cityGatewayService;
        private readonly ILogger<GetCitiesFromStateUseCase> _logger;

        public GetCitiesFromStateUseCase(ICityGatewayService cityGatewayService, ILogger<GetCitiesFromStateUseCase> logger)
        {
            _cityGatewayService = cityGatewayService;
            _logger = logger;
        }

        public Task<IEnumerable<City>?> ExecuteAsync(int param)
        {
            _logger.LogInformation("Getting cities from state identified by {stateId}", param);
            return _cityGatewayService.GetCitiesFromStateAsync(stateId: param);
        }
    }
}
