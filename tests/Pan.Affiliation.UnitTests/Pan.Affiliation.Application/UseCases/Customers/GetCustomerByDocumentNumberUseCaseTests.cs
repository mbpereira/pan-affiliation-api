using AutoBogus;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Pan.Affiliation.Application.UseCases.Customers;
using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.Queries;
using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;
using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Application.UseCases.Customers;

public class GetCustomerByDocumentNumberUseCaseTests
{
    private readonly IGetCustomerByDocumentNumberQuery _query;
    private readonly ILogger<GetCustomerByDocumentNumberUseCase> _logger;
    private readonly IValidationContext _validationContext;
    private readonly Faker _faker = new();

    public GetCustomerByDocumentNumberUseCaseTests()
    {
        _query = Substitute.For<IGetCustomerByDocumentNumberQuery>();
        _logger = Substitute.For<ILogger<GetCustomerByDocumentNumberUseCase>>();
        _validationContext = Substitute.For<IValidationContext>();
    }

    public GetCustomerByDocumentNumberUseCase GetUseCase()
        => new(_query, _logger, _validationContext);

    [Fact]
    public async Task When_ExecuteAsync_is_called_with_invalid_document_should_return_null()
    {
        var useCase = GetUseCase();

        var customer = await useCase.ExecuteAsync(_faker.Random.Word());

        customer.Should().BeNull();
        _validationContext
            .Received()
            .AddNotification(
                Arg.Is<string>(nameof(DocumentNumber)),
                Arg.Is<string>(Shared.Constants.Errors.InvalidDocumentNumberErrorMessage));
    }

    [Theory]
    [InlineData("05915193137")]
    [InlineData("059.151.931-37")]
    [InlineData("91.223.171/0001-35")]
    [InlineData("91223171000135")]
    public async Task When_ExecuteAsync_is_called_with_non_existing_customer_should_return_null(string validDocument)
    {
        var useCase = GetUseCase();

        var customer = await useCase.ExecuteAsync(validDocument);

        customer.Should().BeNull();
        _validationContext
            .Received()
            .SetStatus(Arg.Is(ValidationStatus.NotFound));
    }

    [Theory]
    [InlineData("05915193137")]
    [InlineData("059.151.931-37")]
    [InlineData("91.223.171/0001-35")]
    [InlineData("91223171000135")]
    public async Task When_ExecuteAsync_is_called_with_valid_document_number_should_return_customer_data(string validDocument)
    {
        var customer = new AutoFaker<Customer>()
            .Generate();
        _query.GetCustomerByDocumentNumberAsync(Arg.Is<DocumentNumber>(validDocument))
            .Returns(customer);
        var useCase = GetUseCase();

        var response = await useCase.ExecuteAsync(validDocument);

        response.Should().NotBeNull();
        _validationContext
            .DidNotReceive()
            .SetStatus(Arg.Any<ValidationStatus>());
        _validationContext
            .DidNotReceive()
            .AddNotification(Arg.Any<string>(), Arg.Any<string>());
        response.Should().BeEquivalentTo(customer);
    }
}