using Pan.Affiliation.Domain.Modules.Customers.Entities;

namespace Pan.Affiliation.Domain.Modules.Customers.Commands;

public interface ICreateCustomerCommandHandler
{
    Task<Customer> CreateCustomerAsync(Customer customer);
}