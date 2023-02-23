using AutoBogus;
using Bogus;
using Bogus.Extensions.Brazil;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pan.Affiliation.Infrastructure.Persistence.Repositories;
using Pan.Affiliation.Shared.Extensions;
using Pan.Affiliation.Infrastructure.Persistence;
using DomainCustomer = Pan.Affiliation.Domain.Modules.Customers.Entities.Customer;
using DomainAddress = Pan.Affiliation.Domain.Modules.Customers.Entities.Address;
using InfraCustomer = Pan.Affiliation.Infrastructure.Persistence.Entities.Customer;
using InfraAddress = Pan.Affiliation.Infrastructure.Persistence.Entities.Address;


namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Infrastructure.Persistence.Repositories;

public class CustomersRepositoryTests
{
    private readonly Faker _faker = new();

    [Fact]
    public async Task When_GetCustomerByDocumentNumberAsync_called_Should_return_null_if_customer_was_not_found()
    {
        var customers = new AutoFaker<InfraCustomer>()
            .Generate(1);

        using var context = await GetInMemoryDbContext(async dbContext =>
        {
            await dbContext.Customers!.AddRangeAsync(customers);
            await dbContext.SaveChangesAsync();
        });
        var repository = GetRepository(context);

        var foundCustomer = await repository.GetCustomerByDocumentNumberAsync(_faker.Random.Word());

        foundCustomer.Should().BeNull();
    }

    [Theory]
    [InlineData("45632922065")]
    [InlineData("059.151.931-37")]
    [InlineData("05.679.926/0001-79")]
    [InlineData("38456443000164")]
    public async Task When_GetCustomerByDocumentNumberAsync_called_Should_return_customer_information(
        string validDocument)
    {
        var document = validDocument.OnlyNumbers();
        var customer = new AutoFaker<InfraCustomer>()
            .RuleFor(c => c.Addresses, _ =>
                new AutoFaker<InfraAddress>().Generate(1).ToList())
            .RuleFor(c => c.DocumentNumber, _ => document)
            .Generate();

        using var context = await GetInMemoryDbContext(async dbContext =>
        {
            await dbContext.Customers!.AddAsync(customer);
            await dbContext.SaveChangesAsync();
        });
        var repository = GetRepository(context);

        var foundCustomer = await repository.GetCustomerByDocumentNumberAsync(document);
        var foundAddress = foundCustomer!.Addresses.FirstOrDefault();

        foundCustomer.DocumentNumberVo.Value.Should().Be(customer!.DocumentNumber);
        foundAddress.Should().NotBeNull();
        foundCustomer.Should().NotBeNull();
        foundCustomer.Should().BeEquivalentTo(
            customer,
            opt =>
                opt
                    .Excluding(c => c.Addresses)
                    .Excluding(c => c.DocumentNumber)
                    .ExcludingMissingMembers());
        foundAddress.Should()
            .BeEquivalentTo(
                customer.Addresses!.FirstOrDefault(),
                opt => opt
                    .ExcludingMissingMembers()
                    .Excluding(a => a!.PostalCode));
    }

    [Fact]
    public async Task When_ChangeCustomerAsync_called_Should_return_null_if_customer_was_not_found()
    {
        var customers = GetFakeCustomers();

        using var context = await GetInMemoryDbContext(async dbContext =>
        {
            await dbContext.Customers!.AddRangeAsync(customers);
            await dbContext.SaveChangesAsync();
        });
        var repository = GetRepository(context);

        var changedCustomer = await repository.ChangeCustomerAsync(new(
            Guid.Empty,
            _faker.Random.Word(),
            _faker.Person.Cpf()));

        changedCustomer.Should().BeNull();
    }

    [Fact]
    public async Task When_ChangeCustomerAsync_called_should_change_customer_data_and_return_changed_customer()
    {
        var customers = GetFakeCustomers();

        using var context = await GetInMemoryDbContext(async dbContext =>
        {
            await dbContext.Customers!.AddRangeAsync(customers);
            await dbContext.SaveChangesAsync();
        });
        var repository = GetRepository(context);

        var customer = customers.FirstOrDefault()!.ToDomainEntity();
        var newName = _faker.Person.FirstName;
        var oldName = customer.Name;

        customer.ChangeName(newName);
        await repository.ChangeCustomerAsync(customer);
        var changedCustomer = await repository.GetCustomerByDocumentNumberAsync(customer.DocumentNumber);

        changedCustomer.Should().NotBeNull();
        changedCustomer?.Name.Should().Be(newName);
        changedCustomer?.Name.Should().NotBe(oldName);
    }

    [Fact]
    public async Task When_ChangeCustomerAsync_called_should_add_new_address_to_customer()
    {
        var customers = GetFakeCustomers();
        using var context = await GetInMemoryDbContext(async dbContext =>
        {
            await dbContext.Customers!.AddRangeAsync(customers);
            await dbContext.SaveChangesAsync();
        });
        var repository = GetRepository(context);
        var customer = customers.FirstOrDefault()!.ToDomainEntity();
        var oldAddressesCount = customer.Addresses.Count();
        var address = new AutoFaker<DomainAddress>()
            .RuleFor(
                a => a.PostalCode,
                _ => GetFakeCep())
            .Generate();

        customer.AddAddress(address);
        await repository.ChangeCustomerAsync(customer);
        var changedCustomer = await repository.GetCustomerByDocumentNumberAsync(customer.DocumentNumber);

        changedCustomer.Should().NotBeNull();
        changedCustomer?.Addresses.Count().Should().Be(oldAddressesCount + 1);
    }
    
    [Fact]
    public async Task When_ChangeCustomerAsync_called_should_remove_customer_address()
    {
        var customers = GetFakeCustomers();
        using var context = await GetInMemoryDbContext(async dbContext =>
        {
            await dbContext.Customers!.AddRangeAsync(customers);
            await dbContext.SaveChangesAsync();
        });
        var repository = GetRepository(context);
        var customer = customers.FirstOrDefault()!.ToDomainEntity();
        var oldAddressesCount = customer.Addresses.Count();
        var addressToRemove = customer.Addresses.FirstOrDefault();

        customer.RemoveAddress(addressToRemove!.Id);
        await repository.ChangeCustomerAsync(customer);
        var changedCustomer = await repository.GetCustomerByDocumentNumberAsync(customer.DocumentNumber);

        changedCustomer.Should().NotBeNull();
        changedCustomer?.Addresses.Count().Should().Be(oldAddressesCount - 1);
    }
    
    [Fact]
    public async Task When_ChangeCustomerAsync_called_should_update_customer_address()
    {
        var customers = GetFakeCustomers();
        using var context = await GetInMemoryDbContext(async dbContext =>
        {
            await dbContext.Customers!.AddRangeAsync(customers);
            await dbContext.SaveChangesAsync();
        });
        var repository = GetRepository(context);
        var customer = customers.FirstOrDefault()!.ToDomainEntity();
        var addressToChange = customer.Addresses.FirstOrDefault();
        var oldAddressQt = customer.Addresses.Count();
        var oldStreetName = addressToChange!.Street;
        var newStreetName = _faker.Random.Word();
        addressToChange.Street = newStreetName;

        customer.ChangeAddress(addressToChange);
        await repository.ChangeCustomerAsync(customer);
        var changedCustomer = await repository.GetCustomerByDocumentNumberAsync(customer.DocumentNumber);
        var changedAddress = changedCustomer!.Addresses.FirstOrDefault(a => a.Id == addressToChange.Id);

        changedAddress.Should().NotBeNull();
        changedAddress?.Street.Should().Be(newStreetName);
        changedAddress?.Street.Should().NotBe(oldStreetName);
        changedCustomer.Addresses.Count().Should().Be(oldAddressQt);
    }
    
    [Fact]
    public async Task When_CreateCustomerAsync_called_should_create_customers_and_addresses()
    {
        var customers = GetFakeCustomers();
        using var context = await GetInMemoryDbContext();
        var repository = GetRepository(context);
        var customer = customers.FirstOrDefault()!.ToDomainEntity();

        _ = await repository.CreateCustomerAsync(customer);
        var createdCustomer = await repository.GetCustomerByIdAsync(customer.Id);

        createdCustomer.Should().BeEquivalentTo(customer, opt =>
            opt.Excluding(c => c.Addresses));

        createdCustomer.Addresses.Should().HaveCountGreaterThan(0);
    }

    private string GetFakeCep()
        => _faker.Random.String(minChar: '0', maxChar: '9', length: 8);

    private CustomersRepository GetRepository(PanAffiliationDbContext context)
        => new(context);

    private async Task<PanAffiliationDbContext> GetInMemoryDbContext(Func<PanAffiliationDbContext, Task>? seed = null)
    {
        var options = new DbContextOptionsBuilder<PanAffiliationDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(PanAffiliationDbContext))
            .Options;

        var context = new PanAffiliationDbContext(options);

        if (seed != null)
            await seed(context);

        return context;
    }

    private List<InfraCustomer> GetFakeCustomers(int customersQt = 1, int addressQt = 3)
    {
        var customers = new AutoFaker<InfraCustomer>()
            .RuleFor(
                c => c.Addresses,
                _ => null)
            .RuleFor(
                c => c.DocumentNumber,
                _ => _faker.Person.Cpf(includeFormatSymbols: false))
            .Generate(customersQt);

        foreach (var customer in customers)
        {
            customer.Addresses = new AutoFaker<InfraAddress>()
                .RuleFor(a => a.CustomerId, _ => customer.Id)
                .RuleFor(a => a.PostalCode, _ => GetFakeCep())
                .Generate(addressQt);
        }

        return customers;
    }
}