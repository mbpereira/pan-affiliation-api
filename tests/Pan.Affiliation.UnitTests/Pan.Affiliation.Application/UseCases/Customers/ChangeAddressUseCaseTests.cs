using AutoBogus;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using NSubstitute;
using Pan.Affiliation.Application.UseCases.Customers;
using Pan.Affiliation.Domain.Modules.Customers.Commands;
using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.Queries;
using Pan.Affiliation.Domain.Modules.Customers.UseCases.ChangeAddress;
using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Application.UseCases.Customers;

public class ChangeAddressUseCaseTests
{
    private readonly IChangeCustomerCommandHandler _command;
    private readonly IGetCustomerByIdQueryHandler _query;
    private readonly IValidationContext _validationContext;
    private readonly ILogger<ChangeAddressUseCase> _logger;
    private readonly Faker _faker = new();

    public ChangeAddressUseCaseTests()
    {
        _command = Substitute.For<IChangeCustomerCommandHandler>();
        _query = Substitute.For<IGetCustomerByIdQueryHandler>();
        _validationContext = Substitute.For<IValidationContext>();
        _logger = Substitute.For<ILogger<ChangeAddressUseCase>>();
    }

    [Fact]
    public async Task When_ExecuteAsync_called_should_return_null_if_merchant_was_not_found()
    {
        _query.GetCustomerByIdAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult<Customer?>(null));
        var useCase = GetUseCase();

        var changedAddress = await useCase.ExecuteAsync(new(Guid.NewGuid(), Guid.NewGuid(), null));

        changedAddress.Should().BeNull();
        _validationContext.Received().SetStatus(Arg.Is(ValidationStatus.NotFound));
        _validationContext.Received().AddNotification(Arg.Is<Error>(e =>
            e.Key == nameof(Customer) && e.Message == Shared.Constants.Errors.RecordNotFound));
    }

    [Fact]
    public async Task When_ExecuteAsync_called_should_return_null_if_provided_address_is_not_found()
    {
        var customer = new AutoFaker<Customer>()
            .Generate();
        _query.GetCustomerByIdAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult<Customer?>(customer));
        var address = new AutoFaker<AddressInput>().Generate();
        var input = new ChangeAddressInput(Guid.NewGuid(), Guid.NewGuid(), address);
        var useCase = GetUseCase();

        var changedAddress = await useCase.ExecuteAsync(input);

        changedAddress.Should().BeNull();
        _validationContext.Received().SetStatus(Arg.Is(ValidationStatus.NotFound));
        _validationContext.Received().AddNotification(Arg.Is<Error>(e =>
            e.Key == nameof(AddressInput) && e.Message == Shared.Constants.Errors.RecordNotFound));
    }

    [Fact]
    public async Task When_ExecuteAsync_called_should_return_null_if_provided_address_is_not_valid()
    {
        var address = new AutoFaker<AddressInput>()
            .RuleFor(a => a.State, 
                _ => _faker.Random.String(minChar: 'A', maxChar: 'Z', length: 2))
            .RuleFor(a => a.PostalCode, _ =>
                _faker.Random.String(minChar: '0', maxChar: '9', length: 8))
            .Generate();
        var input = new ChangeAddressInput(Guid.NewGuid(), Guid.NewGuid(), address);
        var addresses = new List<Address> { input.ToDomainAddress() };
        var customer = new Customer(input.CustomerId, _faker.Person.FirstName,
            _faker.Person.Cpf(includeFormatSymbols: false), addresses);
        _query.GetCustomerByIdAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult<Customer?>(customer));
        var useCase = GetUseCase();
        input.Address.Street = null;

        var changedAddress = await useCase.ExecuteAsync(input);

        changedAddress.Should().BeNull();
        _validationContext.DidNotReceive().SetStatus(Arg.Any<ValidationStatus>());
        _validationContext.Received().AddNotifications(Arg.Any<IEnumerable<Error>>());
    }
    
    [Fact]
    public async Task When_ExecuteAsync_called_should_apply_changes()
    {
        var address = new AutoFaker<AddressInput>()
            .RuleFor(a => a.State, 
                _ => _faker.Random.String(minChar: 'A', maxChar: 'Z', length: 2))
            .RuleFor(a => a.PostalCode, _ =>
                _faker.Random.String(minChar: '0', maxChar: '9', length: 8))
            .Generate();
        var input = new ChangeAddressInput(Guid.NewGuid(), Guid.NewGuid(), address);
        var addresses = new List<Address> { input.ToDomainAddress() };
        var customer = new Customer(input.CustomerId, _faker.Person.FirstName,
            _faker.Person.Cpf(includeFormatSymbols: false), addresses);
        _query.GetCustomerByIdAsync(Arg.Any<Guid>())
            .Returns(Task.FromResult<Customer?>(customer));
        var useCase = GetUseCase();
        var oldStreetName = input.Address.Street;
        var newStreetName = _faker.Random.Words(count: 5);
        input.Address.Street = newStreetName;

        var changedAddress = await useCase.ExecuteAsync(input);

        changedAddress.Should().NotBeNull();
        changedAddress.Street.Should().Be(newStreetName);
        changedAddress.Street.Should().NotBe(oldStreetName);
    }

    private ChangeAddressUseCase GetUseCase()
    {
        return new ChangeAddressUseCase(_command, _query, _logger, _validationContext);
    }
}