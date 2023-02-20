using FluentAssertions;
using NSubstitute;
using Pan.Affiliation.Application.UseCases.GetCountryStates;
using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.Logging;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Application.UseCases
{
    public class GetStatesUseCaseTests
    {
        private readonly ICountryStatesService _countryStatesService;
        private readonly ILogger<GetCountryStatesUseCase> _logger;

        public GetStatesUseCaseTests()
        {
            _countryStatesService = Substitute.For<ICountryStatesService>();
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
            _countryStatesService.GetCountryStatesAsync()
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
            _countryStatesService.GetCountryStatesAsync()
                .Returns(Task.FromResult<IEnumerable<State>?>(null));
            var useCase = GetCountryStatesUseCase();

            // act
            var response = await useCase.ExecuteAsync();

            // assert
            response.Should().BeEmpty();
        }

        private GetCountryStatesUseCase GetCountryStatesUseCase()
            => new(_countryStatesService, _logger);
    }
}