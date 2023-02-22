using System.Net;
using AutoBogus;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Pan.Affiliation.Domain.Shared.Caching;
using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.Settings;
using Pan.Affiliation.Infrastructure.Gateways.Ibge;
using Pan.Affiliation.Infrastructure.Gateways.Ibge.Contracts;
using Pan.Affiliation.UnitTests.Utils;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Infrastructure.Gateways.Ibge;

public class IbgeGatewayServiceTests
{
    private readonly ISettingsProvider _settingsProvider;
    private readonly ILogger<IbgeGatewayGatewayFromStateStatesQuery> _logger;
    private readonly Faker _faker = new();
    private readonly ICacheProvider _caching;

    public IbgeGatewayServiceTests()
    {
        _settingsProvider = GetSettingsProvider();
        _logger = Substitute.For<ILogger<IbgeGatewayGatewayFromStateStatesQuery>>();
        _caching = Substitute.For<ICacheProvider>();
    }

    private static ISettingsProvider GetSettingsProvider()
    {
        return new SettingsProviderBuilder()
            .WithEnvironmentVariable($"{Constants.IbgeSettingsKey}__BaseUrl", "https://www.google.com/")
            .Build();
    }

    [Fact]
    public async Task When_GetStatesAsync_Called_should_throws_exception_if_http_response_is_not_success()
    {
        var ibgeClientService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
            GetGatewayService,
            Constants.IbgeHttpClient,
            HttpStatusCode.BadRequest,
            responseContent: "[]");
        IEnumerable<StateResponse>? response = null;
        _caching.GetManyAsync<StateResponse>(Arg.Any<string>())
            .Returns(Task.FromResult(response));

        var act = async () => await ibgeClientService.GetCountryStatesAsync();

        await act.Should().ThrowAsync<HttpRequestException>();
    }
        
    [Fact]
    public async Task When_GetCitiesFromStateAsync_Called_should_throws_exception_if_http_response_is_not_success()
    {
        var ibgeClientService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
            GetGatewayService,
            Constants.IbgeHttpClient,
            HttpStatusCode.BadRequest,
            responseContent: "[]");
        IEnumerable<CityResponse>? response = null;
        _caching.GetManyAsync<CityResponse>(Arg.Any<string>())
            .Returns(Task.FromResult(response));

        var act = async () => await ibgeClientService.GetCitiesFromStateAsync(_faker.Random.Number());

        await act.Should().ThrowAsync<HttpRequestException>();
    }

    [Fact]
    public async Task When_GetStatesAsync_Called_should_return_data_from_cache_if_available()
    {
        var states = new AutoFaker<StateResponse>()
            .Generate(3);
        var ibgeClientService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
            GetGatewayService,
            Constants.IbgeHttpClient,
            responseContent: string.Empty);
        _caching.GetManyAsync<StateResponse>(Arg.Any<string>())
            .Returns(Task.FromResult<IEnumerable<StateResponse>?>(states));

        var response = await ibgeClientService.GetCountryStatesAsync();

        await _caching
            .DidNotReceive()
            .SaveManyAsync(Arg.Any<string>(), Arg.Any<IEnumerable<StateResponse>>());
        response.Should().BeEquivalentTo(states, opt =>
            opt.WithoutStrictOrdering()
        );
    }
        
    [Fact]
    public async Task When_GetCitiesFromStateAsync_Called_should_return_data_from_cache_if_available()
    {
        var cities = new AutoFaker<CityResponse>()
            .Generate(3);
        var ibgeClientService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
            GetGatewayService,
            Constants.IbgeHttpClient,
            responseContent: string.Empty);
        _caching.GetManyAsync<CityResponse>(Arg.Any<string>())
            .Returns(Task.FromResult<IEnumerable<CityResponse>?>(cities));

        var response = await ibgeClientService.GetCitiesFromStateAsync(_faker.Random.Number());

        await _caching
            .DidNotReceive()
            .SaveManyAsync(Arg.Any<string>(), Arg.Any<IEnumerable<StateResponse>>());
        response.Should().BeEquivalentTo(cities, opt =>
            opt.WithoutStrictOrdering()
        );
    }
        
    [Fact]
    public async Task When_GetStatesAsync_Called_should_return_states()
    {
        var states = new AutoFaker<StateResponse>()
            .Generate(3);
        var ibgeClientService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
            GetGatewayService,
            Constants.IbgeHttpClient,
            responseContent: JsonConvert.SerializeObject(states));
        _caching.GetManyAsync<StateResponse>(Arg.Any<string>())
            .Returns(Task.FromResult<IEnumerable<StateResponse>?>(null));

        var response = await ibgeClientService.GetCountryStatesAsync();

        await _caching
            .Received()
            .SaveManyAsync(Arg.Any<string>(), Arg.Any<IEnumerable<StateResponse>>());
        response.Should().BeEquivalentTo(states, opt =>
            opt.WithoutStrictOrdering()
        );
    }

    [Fact]
    public async Task When_CitiesFromState_Called_should_return_cities()
    {
        var cities = new AutoFaker<CityResponse>()
            .Generate(3);
        var ibgeClientService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
            GetGatewayService,
            Constants.IbgeHttpClient,
            responseContent: JsonConvert.SerializeObject(cities));
        _caching.GetManyAsync<CityResponse>(Arg.Any<string>())
            .Returns(Task.FromResult<IEnumerable<CityResponse>?>(null));

        var response = await ibgeClientService.GetCitiesFromStateAsync(_faker.Random.Number());
            
        await _caching
            .Received()
            .SaveManyAsync(Arg.Any<string>(), Arg.Any<IEnumerable<CityResponse>>());
        response.Should().BeEquivalentTo(cities, opt =>
            opt.WithoutStrictOrdering()
        );
    }

    private IbgeGatewayGatewayFromStateStatesQuery GetGatewayService(IHttpClientFactory factory)
        => new(factory, _settingsProvider, _logger, _caching);
}