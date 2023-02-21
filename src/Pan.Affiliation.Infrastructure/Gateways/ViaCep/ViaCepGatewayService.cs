using Pan.Affiliation.Domain.Caching;
using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.Logging;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Domain.ValueObjects;
using Pan.Affiliation.Infrastructure.Gateways.ViaCep.Contracts;
using Pan.Affiliation.Infrastructure.Settings.Sections;

namespace Pan.Affiliation.Infrastructure.Gateways.ViaCep;

public static class Constants
{
    public const string PostalCodeInformationCacheKey = "postal_code_{0}";
    public const string ViaCepSettingsKey = "ViaCepSettings";
    public const string ViaCepHttpClient = "ViaCep";
}

internal static class Resources
{
    public const string GetPostalCodeInformationPath = "ws/{0}/json";
}

public class ViaCepGatewayService : HttpService, IPostalCodeInformationService
{
    private readonly HttpClient _http;
    private readonly ICacheProvider _caching;

    public ViaCepGatewayService(IHttpClientFactory factory,
        ISettingsProvider settingsProvider,
        ILogger<ViaCepGatewayService> logger, ICacheProvider cacheProvider) : base(logger)
    {
        var settings = settingsProvider.GetSection<HttpServiceSettings>(Constants.ViaCepSettingsKey);
        _http = factory.CreateClient(Constants.ViaCepHttpClient);
        _http.BaseAddress = new(settings.BaseUrl!);
        _caching = cacheProvider;
    }

    public async Task<PostalCodeInformation?> GetPostalCodeInformationAsync(PostalCode postalCode)
    {
        var cacheKey = string.Format(Constants.PostalCodeInformationCacheKey, postalCode.Value);

        var cachedValue = await _caching.GetAsync<PostalCodeInformationResponse?>(cacheKey);

        if (cachedValue is not null)
            return cachedValue.ToEntity();

        var path = string.Format(Resources.GetPostalCodeInformationPath, postalCode.ToString());
        var response = await _http.GetAsync(path);

        var postalAddressResponse = await DeserializeResponseAsync<PostalCodeInformationResponse>(response);

        if (postalAddressResponse is null)
            return null;

        await _caching.SaveAsync(cacheKey, postalAddressResponse, TimeSpan.FromHours(1));

        return postalAddressResponse.ToEntity();
    }
}