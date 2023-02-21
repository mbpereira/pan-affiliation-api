using AutoBogus;
using Bogus;
using Bogus.Extensions.Brazil;
using Pan.Affiliation.Infrastructure.Persistence.Entities;

namespace Pan.Affiliation.Infrastructure.Persistence.Helpers;

public class DbSeeder : IDbSeeder
{
    private readonly PanAffiliationDbContext _context;
    private readonly Faker _faker = new();


    public DbSeeder(PanAffiliationDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var customers = new AutoFaker<Customer>()
            .RuleFor(c => c.DocumentNumber, _
                => _faker.Person.Cpf(includeFormatSymbols: false))
            .RuleFor(c => c.Addresses, _ =>
                new AutoFaker<Address>().RuleFor(a => a.PostalCode, _
                        => _faker.Random.String(minChar: '0', maxChar: '9', length: 8))
                    .Generate(1))
            .Generate(5);

        await _context.Customers!.AddRangeAsync(customers);
        await _context.SaveChangesAsync();
    }
}