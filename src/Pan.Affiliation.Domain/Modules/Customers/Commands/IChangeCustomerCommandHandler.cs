using Pan.Affiliation.Domain.Modules.Customers.Entities;

namespace Pan.Affiliation.Domain.Modules.Customers.Commands;

public interface IChangeCustomerCommandHandler
{
    Task<Customer?> ChangeCustomerAsync(Customer customer);
}