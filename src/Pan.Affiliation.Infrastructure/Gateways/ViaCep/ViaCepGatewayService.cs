using Pan.Affiliation.Domain.Caching;
using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.Logging;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Domain.ValueObjects;
using Pan.Affiliation.Infrastructure.Gateways.ViaCep.Contracts;
using Pan.Affiliation.Infrastructure.Settings.Sections;
using static Pan.Affiliation.Shared.Constants.Configuration;
using static Pan.Affiliation.Shared.Constants.HttpClientConfiguration;
using static Pan.Affiliation.Shared.Constants.HttpServices.ViaCep;

namespace Pan.Affiliation.Infrastructure.Gateways.ViaCep;

public class ViaCepGatewayService : HttpService, IPostalCodeInformationService
{
    public static class Constants
    {
        public const string PostalCodeInformationCacheKey = "psotal_code_{0}";
    }

    private readonly HttpClient _http;
    private readonly HttpServiceSettings _settings;
    private readonly ICacheProvider _caching;

    public ViaCepGatewayService(IHttpClientFactory factory,
        ISettingsProvider settingsProvider,
        ILogger<ViaCepGatewayService> logger, ICacheProvider cacheProvider) : base(logger)
    {
        _settings = settingsProvider.GetSection<HttpServiceSettings>(ViaCepSettingsKey);
        _http = factory.CreateClient(ViaCepClient);
        _http.BaseAddress = new(_settings.BaseUrl!);
        _caching = cacheProvider;
    }

    public async Task<PostalCodeInformation?> GetPostalCodeInformationAsync(PostalCode postalCode)
    {
        var cacheKey = string.Format(Constants.PostalCodeInformationCacheKey, postalCode.Value);
        
        var cachedValue = await _caching.GetAsync<PostalCodeInformationResponse?>(cacheKey);

        if (cachedValue is not null)
            return cachedValue.ToEntity();

        var path = string.Format(GetPostalCodeInformationPath, postalCode.ToString());
        var response = await _http.GetAsync(path);

        var postalAddressResponse = await DeserializeResponseAsync<PostalCodeInformationResponse>(response);

        await _caching.SaveAsync(cacheKey, postalAddressResponse, TimeSpan.FromHours(1));
        
        return postalAddressResponse?.ToEntity();
    }
}