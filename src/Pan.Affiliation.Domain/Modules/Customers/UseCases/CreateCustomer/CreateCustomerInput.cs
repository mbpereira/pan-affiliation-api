using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.UseCases.ChangeAddress;

namespace Pan.Affiliation.Domain.Modules.Customers.UseCases.CreateCustomer;

public record CreateCustomerInput(string Name, string DocumentNumber, IEnumerable<AddressInput>? Addresses)
{
    public Customer ToDomainEntity() =>
        new(id: null,
            Name, 
            DocumentNumber, 
            Addresses?.Select(a => a.ToDomainEntity()).ToList());
}