using System.Net;
using AutoBogus;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Infrastructure.Gateways.Ibge;
using Pan.Affiliation.Infrastructure.Gateways.Ibge.Contracts;
using Pan.Affiliation.UnitTests.Utils;
using static Pan.Affiliation.Shared.Constants.Configuration;
using static Pan.Affiliation.Shared.Constants.HttpClientConfiguration;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Infrastructure.Gateways.Ibge
{
    public class IbgeGatewayServiceTests
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly Faker _faker = new();

        public IbgeGatewayServiceTests()
        {
            _settingsProvider = GetSettingsProvider();
        }

        private static ISettingsProvider GetSettingsProvider()
        {
            return new SettingsProviderBuilder()
                .WithEnvironmentVariable($"{IbgeSettingsKey}__BaseUrl", "https://www.google.com/")
                .Build();
        }

        [Fact]
        public async Task When_GetStatesAsync_Called_should_throws_exception_if_http_response_is_not_success()
        {
            var ibgeClientService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
                factory => new IbgeGatewayService(factory, _settingsProvider),
                IbgeClient,
                HttpStatusCode.BadRequest,
                responseContent: "[]");

            var act = async () => await ibgeClientService.GetCountryStatesAsync();

            await act.Should().ThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task When_GetStatesAsync_Called_should_return_states()
        {
            var states = new AutoFaker<StateResponse>()
                .Generate(3);

            var ibgeClientService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
                factory => new IbgeGatewayService(factory, _settingsProvider),
                IbgeClient,
                HttpStatusCode.OK,
                responseContent: JsonConvert.SerializeObject(states));

            var response = await ibgeClientService.GetCountryStatesAsync();

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
                factory => new IbgeGatewayService(factory, _settingsProvider),
                IbgeClient,
                HttpStatusCode.OK,
                responseContent: JsonConvert.SerializeObject(cities));

            var response = await ibgeClientService.GetCitiesFromStateAsync(_faker.Random.Number());

            response.Should().BeEquivalentTo(cities, opt =>
                opt.WithoutStrictOrdering()
            );
        }
    }
}
