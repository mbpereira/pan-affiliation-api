using AutoBogus;
using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pan.Affiliation.Infrastructure.Persistence;
using Pan.Affiliation.Infrastructure.Persistence.Entities;
using Pan.Affiliation.Infrastructure.Persistence.Repositories;
using Pan.Affiliation.Shared.Extensions;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Infrastructure.Persistence.Repositories;

public class CustomerRepositoryTests
{
    private readonly Faker _faker = new();

    [Fact]
    public async Task When_GetCustomerByDocumentNumberAsync_called_Should_return_null_if_customer_was_not_found()
    {
        var customers = new AutoFaker<Customer>()
            // .RuleFor(c => c.DocumentNumber, _ => )
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
        var customer = new AutoFaker<Customer>()
            .RuleFor(c => c.Addresses, _ =>
                new AutoFaker<Address>().Generate(1).ToList())
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
                opt=> opt
                    .ExcludingMissingMembers()
                    .Excluding(a => a!.PostalCode));
    }

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
}