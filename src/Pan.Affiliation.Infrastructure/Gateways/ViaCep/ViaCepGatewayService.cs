using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;
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
    private readonly HttpClient _http;
    private readonly HttpServiceSettings _settings;

    public ViaCepGatewayService(IHttpClientFactory factory, ISettingsProvider settingsProvider)
    {
        _settings = settingsProvider.GetSection<HttpServiceSettings>(ViaCepSettingsKey);
        _http = factory.CreateClient(ViaCepClient);
        _http.BaseAddress = new(_settings.BaseUrl!);
    }

    public async Task<PostalCodeInformation?> GetPostalCodeInformationAsync(PostalCode postalCode)
    {
        var path = string.Format(GetPostalCodeInformationPath, postalCode.ToString());
        var response = await _http.GetAsync(path);

        var postalAddressResponse = await DeserializeResponseAsync<PostalCodeInformationResponse>(response);

        return postalAddressResponse?.ToEntity();
    }
}