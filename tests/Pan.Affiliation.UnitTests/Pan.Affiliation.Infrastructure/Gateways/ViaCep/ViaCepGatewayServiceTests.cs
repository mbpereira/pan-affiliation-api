using System.Net;
using AutoBogus;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Pan.Affiliation.Domain.Caching;
using Pan.Affiliation.Domain.Logging;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Infrastructure.Gateways.Ibge;
using Pan.Affiliation.Infrastructure.Gateways.ViaCep;
using Pan.Affiliation.Infrastructure.Gateways.ViaCep.Contracts;
using Pan.Affiliation.UnitTests.Utils;
using static Pan.Affiliation.Shared.Constants.HttpClientConfiguration;
using static Pan.Affiliation.Shared.Constants.Configuration;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Infrastructure.Gateways.ViaCep;

public class ViaCepGatewayServiceTests
{
    private readonly ISettingsProvider _settingsProvider;
    private readonly ILogger<ViaCepGatewayService> _logger;
    private readonly ICacheProvider _caching;
    private readonly Faker _faker = new();

    public ViaCepGatewayServiceTests()
    {
        _settingsProvider = GetSettingsProvider();
        _logger = Substitute.For<ILogger<ViaCepGatewayService>>();
        _caching = Substitute.For<ICacheProvider>();
    }

    [Fact]
    public async Task
        When_GetPostalCodeInformationAsync_Called_should_throws_exception_if_http_response_is_not_success()
    {
        var viaCepGatewayService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
            GetGatewayService,
            ViaCepClient,
            HttpStatusCode.BadRequest,
            responseContent: "[]");
        var fakeCep = _faker.Random.String(minChar: '0', maxChar: '9', length: 8);
        _caching.GetAsync<PostalCodeInformationResponse>(Arg.Any<string>())
            .Returns(Task.FromResult<PostalCodeInformationResponse?>(null));

        var act = async () => await viaCepGatewayService.GetPostalCodeInformationAsync(fakeCep);

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task When_GetPostalCodeInformationAsync_Called_without_errors_should_return_information()
    {
        var information = new AutoFaker<PostalCodeInformationResponse>()
            .Generate();
        var viaCepGatewayService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
            GetGatewayService,
            ViaCepClient,
            responseContent: JsonConvert.SerializeObject(information));
        var fakeCep = _faker.Random.String(minChar: '0', maxChar: '9', length: 8);
        _caching.GetAsync<PostalCodeInformationResponse>(Arg.Any<string>())
            .Returns(Task.FromResult<PostalCodeInformationResponse?>(null));

        var response = await viaCepGatewayService.GetPostalCodeInformationAsync(fakeCep);

        await _caching.Received().GetAsync<PostalCodeInformationResponse>(Arg.Any<string>());
        response.Should().BeEquivalentTo(information);
    }
    
    [Fact]
    public async Task When_GetPostalCodeInformationAsync_Called_without_errors_should_return_information_from_cache_if_available()
    {
        var information = new AutoFaker<PostalCodeInformationResponse>()
            .Generate();
        var viaCepGatewayService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
            GetGatewayService,
            ViaCepClient,
            responseContent: "");
        var fakeCep = _faker.Random.String(minChar: '0', maxChar: '9', length: 8);
        _caching.GetAsync<PostalCodeInformationResponse>(Arg.Any<string>())
            .Returns(Task.FromResult<PostalCodeInformationResponse?>(information));

        var response = await viaCepGatewayService.GetPostalCodeInformationAsync(fakeCep);

        await _caching
            .DidNotReceive()
            .SaveAsync<PostalCodeInformationResponse>(
                Arg.Any<string>(), 
                Arg.Any<PostalCodeInformationResponse>(),
                Arg.Any<TimeSpan>());
        response.Should().BeEquivalentTo(information);
    }

    [Fact]
    public async Task When_viacep_gives_null_response_should_return_null()
    {
        var viaCepGatewayService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
            GetGatewayService,
            ViaCepClient,
            responseContent: "");
        var fakeCep = _faker.Random.String(minChar: '0', maxChar: '9', length: 8);
        _caching.GetAsync<PostalCodeInformationResponse>(Arg.Any<string>())
            .Returns(Task.FromResult<PostalCodeInformationResponse?>(null));
        
        var response = await viaCepGatewayService.GetPostalCodeInformationAsync(fakeCep);
        
        await _caching.Received().GetAsync<PostalCodeInformationResponse>(Arg.Any<string>());
        response.Should().BeNull();
    }

    private static ISettingsProvider GetSettingsProvider()
    {
        return new SettingsProviderBuilder()
            .WithEnvironmentVariable($"{ViaCepSettingsKey}__BaseUrl", "https://www.google.com/")
            .Build();
    }

    private ViaCepGatewayService GetGatewayService(IHttpClientFactory factory)
        => new(factory, _settingsProvider, _logger, _caching);
}