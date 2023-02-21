using Pan.Affiliation.Domain.Caching;
using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.Logging;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Infrastructure.Gateways.Ibge.Contracts;
using Pan.Affiliation.Infrastructure.Settings.Sections;

namespace Pan.Affiliation.Infrastructure.Gateways.Ibge;

public static class Constants
{
    public const string IbgeSettingsKey = "IbgeSettings";
    public const string StateCitiesCacheKey = "state_{0}_cities";
    public const string CountryStatesCacheKey = "state_{0}_cities";
    public const string IbgeHttpClient = "IBGE";
}

internal static class Resources
{
    public const string GetSatesPath = "estados";
    public const string GetCitiesFromStatePath = "estados/{0}/municipios";
}

public class IbgeGatewayService : HttpService, ICityService, ICountryStatesService
{
    private readonly HttpClient _http;
    private readonly ICacheProvider _caching;

    public IbgeGatewayService(
        IHttpClientFactory factory,
        ISettingsProvider settingsProvider,
        ILogger<IbgeGatewayService> logger,
        ICacheProvider caching) : base(logger)
    {
        _caching = caching;
        var settings = settingsProvider.GetSection<HttpServiceSettings>(Constants.IbgeSettingsKey);
        _http = factory.CreateClient(Constants.IbgeHttpClient);
        _http.BaseAddress = new Uri(settings.BaseUrl!);
    }

    public async Task<IEnumerable<City>?> GetCitiesFromStateAsync(int stateId)
    {
        var cacheKey = string.Format(Constants.StateCitiesCacheKey, stateId);

        var cachedResult = await _caching.GetManyAsync<CityResponse>(cacheKey);

        if (cachedResult is not null)
            return MapCitiesToEntity(cachedResult);

        var responseMessage = await _http.GetAsync(string.Format(Resources.GetCitiesFromStatePath, stateId));

        var cities = await DeserializeResponseAsync<IEnumerable<CityResponse>>(responseMessage);

        if (cities is null)
            return Enumerable.Empty<City>();

        var response = cities.ToList();
        
        await _caching.SaveManyAsync(cacheKey, response);

        return MapCitiesToEntity(response);
    }

    private static IEnumerable<City>? MapCitiesToEntity(IEnumerable<CityResponse>? cities)
    {
        return cities?.Select(s => s.ToEntity());
    }

    public async Task<IEnumerable<State>?> GetCountryStatesAsync()
    {
        var cacheKey = Constants.CountryStatesCacheKey;

        var cachedResult = await _caching.GetManyAsync<StateResponse>(cacheKey);

        if (cachedResult is not null)
            return MapStatesToEntity(cachedResult);

        var responseMessage = await _http.GetAsync(Resources.GetSatesPath);

        var states = await DeserializeResponseAsync<IEnumerable<StateResponse>>(responseMessage);

        if (states is null)
            return Enumerable.Empty<State>();

        var response = states.ToList();
        await _caching.SaveManyAsync(cacheKey, response);

        return MapStatesToEntity(response);
    }

    private static IEnumerable<State>? MapStatesToEntity(IEnumerable<StateResponse>? states)
    {
        return states?.Select(s => s.ToEntity());
    }
}