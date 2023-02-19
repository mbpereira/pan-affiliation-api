using AutoBogus;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using Pan.Affiliation.Domain.Settings;
using Pan.Affiliation.Infrastructure.HttpServices.Ibge.Contracts;
using Pan.Affiliation.Infrastructure.Services.Ibge;
using Pan.Affiliation.Infrastructure.Settings;
using Pan.Affiliation.UnitTests.Utils;
using System.Net;
using static Pan.Affiliation.Shared.Constants.Configuration;
using static Pan.Affiliation.Shared.Constants.HttpClientConfiguration;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Infrastructure.HttpServices.Ibge
{
    public class IbgeServiceTests
    {
        private readonly ISettingsProvider _settingsProvider;
        private readonly Faker _faker = new();

        public IbgeServiceTests()
        {
            _settingsProvider = GetSettingsProvider();
        }

        private static SettingsProvider GetSettingsProvider()
        {
            var config = new CustomConfigurationBuilder()
                .WithEnvironmentVariable($"{IbgeSettingsKey}__BaseUrl", "https://www.google.com/")
                .Build();

            return new SettingsProvider(config);
        }

        [Fact]
        public async Task When_GetStatesAsync_Called_should_throws_exception_if_http_response_is_not_success()
        {
            var ibgeClientService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
                factory => new IbgeService(factory, _settingsProvider),
                IbgeClient,
                HttpStatusCode.BadRequest,
                responseContent: "[]");

            var act = async () => await ibgeClientService.GetStatesAsync();

            await act.Should().ThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task When_GetStatesAsync_Called_should_return_states()
        {
            var states = new AutoFaker<StateResponse>()
                .Generate(3);

            var ibgeClientService = HttpClientFactoryUtils.CreateMockedHttpClientFactory(
                factory => new IbgeService(factory, _settingsProvider),
                IbgeClient,
                HttpStatusCode.OK,
                responseContent: JsonConvert.SerializeObject(states));

            var response = await ibgeClientService.GetStatesAsync();

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
                factory => new IbgeService(factory, _settingsProvider),
                IbgeClient,
                HttpStatusCode.OK,
                responseContent: JsonConvert.SerializeObject(cities));

            var response = await ibgeClientService.GetCitiesFromState(_faker.Random.Number());

            response.Should().BeEquivalentTo(cities, opt =>
                opt.WithoutStrictOrdering()
            );
        }
    }
}
