using Pan.Affiliation.Domain.Caching;
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
        public static class Constants
        {
            public const string StateCitiesKey = "state_{0}_cities";
            public const string CountryStatesKey = "state_{0}_cities";
        }

        private readonly HttpClient _http;
        private readonly HttpServiceSettings _settings;
        private readonly ICacheProvider _caching;

        public IbgeGatewayService(
            IHttpClientFactory factory,
            ISettingsProvider settingsProvider,
            ILogger<IbgeGatewayService> logger,
            ICacheProvider caching) : base(logger)
        {
            _caching = caching;
            _settings = settingsProvider.GetSection<HttpServiceSettings>(IbgeSettingsKey);
            _http = factory.CreateClient(IbgeClient);
            _http.BaseAddress = new Uri(_settings.BaseUrl!);
        }

        public async Task<IEnumerable<City>?> GetCitiesFromStateAsync(int stateId)
        {
            var cacheKey = string.Format(Constants.StateCitiesKey, stateId);
            
            var cachedResult = await _caching.GetManyAsync<CityResponse>(cacheKey);

            if (cachedResult is not null)
                return MapCitiesToEntity(cachedResult);
            
            var response = await _http.GetAsync(string.Format(GetCitiesFromStatePath, stateId));

            var cities = await DeserializeResponseAsync<IEnumerable<CityResponse>>(response);

            await _caching.SaveManyAsync(cacheKey, cities);

            return MapCitiesToEntity(cities);
        }

        private static IEnumerable<City>? MapCitiesToEntity(IEnumerable<CityResponse> cities)
        {
            return cities?.Select(s => s.ToEntity());
        }

        public async Task<IEnumerable<State>?> GetCountryStatesAsync()
        {
            var cacheKey = Constants.CountryStatesKey;
            
            var cachedResult = await _caching.GetManyAsync<StateResponse>(cacheKey);

            if (cachedResult is not null)
                return MapStatesToEntity(cachedResult);
            
            var response = await _http.GetAsync(GetSatesPath);
            
            var states = await DeserializeResponseAsync<IEnumerable<StateResponse>>(response);

            await _caching.SaveManyAsync(cacheKey, states);

            return MapStatesToEntity(states);
        }

        private static IEnumerable<State>? MapStatesToEntity(IEnumerable<StateResponse> states)
        {
            return states?.Select(s => s.ToEntity());
        }
    }
}