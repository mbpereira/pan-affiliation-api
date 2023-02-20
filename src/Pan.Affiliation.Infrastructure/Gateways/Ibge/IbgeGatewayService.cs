using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.Logging;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Infrastructure.Gateways.Ibge.Contracts;
using Pan.Affiliation.Infrastructure.Settings.Sections;
using static Pan.Affiliation.Shared.Constants.HttpClientConfiguration;
using static Pan.Affiliation.Shared.Constants.HttpServices.Ibge;
using static Pan.Affiliation.Shared.Constants.Configuration;

namespace Pan.Affiliation.Infrastructure.Gateways.Ibge
{
    public class IbgeGatewayService : HttpService, ICityService, ICountryStatesService
    {
        private readonly HttpClient _http;
        private readonly HttpServiceSettings _settings;

        public IbgeGatewayService(
            IHttpClientFactory factory, 
            ISettingsProvider settingsProvider,
            ILogger<IbgeGatewayService> logger) : base(logger)
        {
            _settings = settingsProvider.GetSection<HttpServiceSettings>(IbgeSettingsKey);
            _http = factory.CreateClient(IbgeClient);
            _http.BaseAddress = new Uri(_settings.BaseUrl!);
        }

        public async Task<IEnumerable<City>?> GetCitiesFromStateAsync(int stateId)
        {
            var response = await _http.GetAsync(string.Format(GetCitiesFromStatePath, stateId));

            var cities = await DeserializeResponseAsync<IEnumerable<CityResponse>>(response);

            return cities?.Select(s => s.ToEntity());
        }

        public async Task<IEnumerable<State>?> GetCountryStatesAsync()
        {
            var response = await _http.GetAsync(GetSatesPath);

            var states = await DeserializeResponseAsync<IEnumerable<StateResponse>>(response);

            return states?.Select(s => s.ToEntity());
        }
    }
}
