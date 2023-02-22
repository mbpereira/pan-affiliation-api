using FluentAssertions;
using NSubstitute;
using Pan.Affiliation.Application.UseCases.Customers;
using Pan.Affiliation.Domain.Modules.Customers.Queries;
using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Application.UseCases.Customers;

public class GetAllCustomersUseCaseTests
{
    private readonly IGetAllCustomersQueryHandler _query;
    private readonly IValidationContext _validationContext;
    private readonly ILogger<GetAllCustomersUseCase> _logger;

    public GetAllCustomersUseCaseTests()
    {
        _query = Substitute.For<IGetAllCustomersQueryHandler>();
        _validationContext = Substitute.For<IValidationContext>();
        _logger = Substitute.For<ILogger<GetAllCustomersUseCase>>();
    }
    
    [Theory]
    [InlineData(1, 0, 25)]
    [InlineData(2, 25, 25)]
    [InlineData(3, 50, 25)]
    public async Task When_ExecuteAsync_called_should_get_data_from_correct_page(int page, int expectedSkip, int expectedTake)
    {
        var useCase = GetUseCase();

        _ = await useCase.ExecuteAsync(page);

        await _query
            .Received()
            .GetAllCustomersAsync(Arg.Is(expectedSkip), Arg.Is(expectedTake));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    public async Task When_ExecuteAsync_called_should_return_null_if_page_is_less_or_equal_than_0(int page)
    {
        var useCase = GetUseCase();

        var response = await useCase.ExecuteAsync(page);

        response.Should().BeNull();
        _validationContext.Received()
            .AddNotification(
                Arg.Is(nameof(Shared.Constants.Errors.InvalidPageError)), 
                Arg.Is(Shared.Constants.Errors.InvalidPageError));
    }

    private GetAllCustomersUseCase GetUseCase() => new(_query, _validationContext, _logger);
}