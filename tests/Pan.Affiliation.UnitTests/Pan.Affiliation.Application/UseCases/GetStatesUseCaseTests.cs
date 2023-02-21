using FluentAssertions;
using NSubstitute;
using Pan.Affiliation.Application.UseCases.GetCountryStates;
using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Modules.Localization.Gateways;
using Pan.Affiliation.Domain.Shared.Logging;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Application.UseCases
{
    public class GetStatesUseCaseTests
    {
        private readonly ICountryStatesGatewayService _countryStatesGatewayService;
        private readonly ILogger<GetCountryStatesUseCase> _logger;

        public GetStatesUseCaseTests()
        {
            _countryStatesGatewayService = Substitute.For<ICountryStatesGatewayService>();
            _logger = Substitute.For<ILogger<GetCountryStatesUseCase>>();
        }

        [Fact]
        public async Task When_ExecuteAsync_is_called_should_return_SP_and_RJ_at_first_position()
        {
            // arrange
            var states = new List<State>()
            {
                new() { Name = "Mato Grosso", Acronym = "MT" },
                new() { Name = "Rio Grande do Sul", Acronym = "RS" },
                new() { Name = "Bahia", Acronym = "BA" },
                new() { Name = "Rio de Janeiro", Acronym = "RJ" },
                new() { Name = "São Paulo", Acronym = "SP" }
            };
            _countryStatesGatewayService.GetCountryStatesAsync()
                .Returns(states);
            var useCase = GetCountryStatesUseCase();

            // act
            var response = await useCase.ExecuteAsync();

            // assert
            response.Should().BeEquivalentTo(new List<State>
            {
                new() { Name = "São Paulo", Acronym = "SP" },
                new() { Name = "Rio de Janeiro", Acronym = "RJ" },
                new() { Name = "Bahia", Acronym = "BA" },
                new() { Name = "Mato Grosso", Acronym = "MT" },
                new() { Name = "Rio Grande do Sul", Acronym = "RS" },
            }, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task When_ExecuteAsync_is_called_should_return_empty_if_response_from_service_is_null()
        {
            // arrange
            _countryStatesGatewayService.GetCountryStatesAsync()
                .Returns(Task.FromResult<IEnumerable<State>?>(null));
            var useCase = GetCountryStatesUseCase();

            // act
            var response = await useCase.ExecuteAsync();

            // assert
            response.Should().BeEmpty();
        }

        private GetCountryStatesUseCase GetCountryStatesUseCase()
            => new(_countryStatesGatewayService, _logger);
    }
}