using FluentAssertions;
using NSubstitute;
using Pan.Affiliation.Application.UseCases.GetStates;
using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Application.UseCases
{
    public class GetStatesUseCaseTests
    {
        private readonly ICountryStatesService _countryStatesService;

        public GetStatesUseCaseTests()
        {
            _countryStatesService = Substitute.For<ICountryStatesService>();
        }

        [Fact]
        public async Task When_ExecuteAsync_is_called_should_return_SP_and_RJ_at_first_position()
        {
            // arrange
            var states = new List<State>()
            {
                new State { Name = "Mato Grosso", Acronym = "MT" },
                new State { Name = "Rio Grande do Sul", Acronym = "RS" },
                new State { Name = "Bahia", Acronym = "BA" },
                new State { Name = "Rio de Janeiro", Acronym = "RJ" },
                new State { Name = "São Paulo", Acronym = "SP" }
            };
            _countryStatesService.GetCountryStatesAsync()
                .Returns(states);
            var useCase = new GetCountryStatesUseCase(_countryStatesService);

            // act
            var response = await useCase.ExecuteAsync();

            // assert
            response.Should().BeEquivalentTo(new List<State>
            {
                new State { Name = "São Paulo", Acronym = "SP" },
                new State { Name = "Rio de Janeiro", Acronym = "RJ" },
                new State { Name = "Bahia", Acronym = "BA" },
                new State { Name = "Mato Grosso", Acronym = "MT" },
                new State { Name = "Rio Grande do Sul", Acronym = "RS" },
            }, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public async Task When_ExecuteAsync_is_called_should_return_empty_if_response_from_service_is_null()
        {
            // arrange
            _countryStatesService.GetCountryStatesAsync()
                .Returns(Task.FromResult<IEnumerable<State>?>(null));
            var useCase = new GetCountryStatesUseCase(_countryStatesService);

            // act
            var response = await useCase.ExecuteAsync();

            // assert
            response.Should().BeEmpty();
        }
    }
}
