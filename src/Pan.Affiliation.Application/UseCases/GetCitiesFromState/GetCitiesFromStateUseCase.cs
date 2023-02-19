using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.Application.UseCases.GetCitiesFromState
{
    public class GetCitiesFromStateUseCase : IGetCitiesFromStateUseCase
    {
        private readonly ICityService _cityService;

        public GetCitiesFromStateUseCase(ICityService cityService)
        {
            _cityService = cityService;
        }

        public Task<IEnumerable<City>?> ExecuteAsync(int param)
        {
            return _cityService.GetCitiesFromStateAsync(stateId: param);
        }
    }
}
