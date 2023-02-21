using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.Logging;

namespace Pan.Affiliation.Application.UseCases.GetCountryStates
{
    public class GetCountryStatesUseCase : IGetCountryStatesUseCase
    {
        private readonly ICountryStatesService _countryStatesService;
        private readonly ILogger<GetCountryStatesUseCase> _logger;
        
        public GetCountryStatesUseCase(ICountryStatesService countryStateService, ILogger<GetCountryStatesUseCase> logger)
        {
            _countryStatesService = countryStateService;
            _logger = logger;
        }

        public async Task<IEnumerable<State>?> ExecuteAsync()
        {
            _logger.LogInformation("Getting country states");
            
            var statesResponse = await _countryStatesService.GetCountryStatesAsync();

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
