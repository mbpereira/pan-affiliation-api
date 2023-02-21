using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Modules.Localization.Queries;
using Pan.Affiliation.Domain.Modules.Localization.UseCases;
using Pan.Affiliation.Domain.Shared.Logging;

namespace Pan.Affiliation.Application.UseCases.Localization
{
    public class GetCountryStatesUseCase : IGetCountryStatesUseCase
    {
        private readonly IGetCountryStatesQuery _getCountryStatesQuery;
        private readonly ILogger<GetCountryStatesUseCase> _logger;
        
        public GetCountryStatesUseCase(IGetCountryStatesQuery getCountryStatesStateGatewayService, ILogger<GetCountryStatesUseCase> logger)
        {
            _getCountryStatesQuery = getCountryStatesStateGatewayService;
            _logger = logger;
        }

        public async Task<IEnumerable<State>?> ExecuteAsync()
        {
            _logger.LogInformation("Getting country states");
            
            var statesResponse = await _getCountryStatesQuery.GetCountryStatesAsync();

            if (statesResponse is null)
                return Enumerable.Empty<State>();

            var priorityStates = new[] { "RJ", "SP" };

            var states = statesResponse.ToList();
            var dict = states.ToDictionary(s => s.Acronym!, StringComparer.InvariantCultureIgnoreCase);

            var response = states
                .Where(s => !priorityStates.Contains(s.Acronym!))
                .OrderByDescending(s => s.Name)
                .ToList();

            foreach (var priority in priorityStates)
            {
                if (dict.TryGetValue(priority, out var state))
                    response.Add(state);
            }

            response.Reverse();

            return response;
        }
    }
}
