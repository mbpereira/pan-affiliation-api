using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using NSubstitute;
using Pan.Affiliation.Application.UseCases.Customers;
using Pan.Affiliation.Domain.Modules.Customers.Commands;
using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.Queries;
using Pan.Affiliation.Domain.Modules.Customers.UseCases.CreateCustomer;
using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;
using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Application.UseCases.Customers;

public class CreateCustomerUseCaseTests
{
    private readonly IValidationContext _context;
    private readonly ICreateCustomerCommandHandler _command;
    private readonly ILogger<CreateCustomerUseCase> _logger;
    private readonly IGetCustomerByDocumentNumberQueryHandler _query;
    private readonly Faker _faker = new();


    public CreateCustomerUseCaseTests()
    {
        _context = Substitute.For<IValidationContext>();
        _command = Substitute.For<ICreateCustomerCommandHandler>();
        _logger = Substitute.For<ILogger<CreateCustomerUseCase>>();
        _query = Substitute.For<IGetCustomerByDocumentNumberQueryHandler>();
    }

    public CreateCustomerUseCase GetUseCase()
        => new(_logger, _command, _context, _query);


    [Fact]
    async Task When_ExecuteAsync_called_with_invalid_input_data_should_return_null()
    {
        var customer = new CreateCustomerInput(null, null, null);
        var useCase = GetUseCase();

        var result = await useCase.ExecuteAsync(customer);

        result.Should().BeNull();
        _context.Received()
            .AddNotifications(Arg.Any<IEnumerable<Error>>());
    }

    [Fact]
    async Task When_ExecuteAsync_called_with_existing_customer_should_return_conflict()
    {
        var customer = new Customer(Guid.NewGuid(), _faker.Random.Words(), _faker.Person.Cpf());
        _query.GetCustomerByDocumentNumberAsync(Arg.Any<DocumentNumber>())
            .Returns(customer);
        var createCustomerInput = new CreateCustomerInput(customer.Name, customer.DocumentNumber, null);
        var useCase = GetUseCase();

        var result = await useCase.ExecuteAsync(createCustomerInput);

        result.Should().BeNull();
        _context.Received().SetStatus(ValidationStatus.Conflict);
        _context.Received()
            .AddNotification(
                Arg.Is(nameof(Shared.Constants.Errors.ConflictError)),
                Arg.Is(Shared.Constants.Errors.ConflictError));
    }

    [Fact]
    async Task When_ExecuteAsync_called_with_valid_data_should_create_customer()
    {
        var customer = new Customer(Guid.NewGuid(), _faker.Random.Words(), _faker.Person.Cpf());
        var createCustomerInput = new CreateCustomerInput(customer.Name, customer.DocumentNumber, null);
        var useCase = GetUseCase();

        _ = await useCase.ExecuteAsync(createCustomerInput);

        _context.DidNotReceive().SetStatus(Arg.Any<ValidationStatus>());
        _context.DidNotReceive()
            .AddNotification(
                Arg.Any<string>(),
                Arg.Any<string>());
        await _command.Received().CreateCustomerAsync(Arg.Any<Customer>());
    }
}