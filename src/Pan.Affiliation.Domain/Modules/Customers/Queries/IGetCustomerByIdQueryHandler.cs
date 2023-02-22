using Pan.Affiliation.Domain.Modules.Customers.Entities;

namespace Pan.Affiliation.Domain.Modules.Customers.Queries;

public interface IGetCustomerByIdQueryHandler
{
    Task<Customer?> GetCustomerByIdAsync(Guid id);
}