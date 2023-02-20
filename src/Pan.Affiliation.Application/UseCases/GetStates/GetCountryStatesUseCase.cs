using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Application.UseCases.GetStates
{
    public class GetCountryStatesUseCase : IGetCountryStatesUseCase
    {
        private readonly ICountryStatesService _countryStatesService;

        public GetCountryStatesUseCase(ICountryStatesService countryStateService)
        {
            _countryStatesService = countryStateService;
        }

        public async Task<IEnumerable<State>?> ExecuteAsync()
        {
            var states = await _countryStatesService.GetCountryStatesAsync();

            if (states is null)
                return Enumerable.Empty<State>();

            var priorityStates = new[] { "RJ", "SP" };

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
