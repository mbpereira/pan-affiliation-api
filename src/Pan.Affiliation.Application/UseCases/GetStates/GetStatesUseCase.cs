using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Application.UseCases.GetStates
{
    public class GetStatesUseCase : IGetStatesUseCase
    {
        private readonly ICountryStatesService _countryStatesService;

        public GetStatesUseCase(ICountryStatesService countryStateService)
        {
            _countryStatesService = countryStateService;
        }

        public Task<IEnumerable<State>?> ExecuteAsync()
        {
            return _countryStatesService.GetCountryStatesAsync();
        }
    }
}
