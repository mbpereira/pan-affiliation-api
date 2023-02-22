using Pan.Affiliation.Domain.Modules.Customers.Entities;

namespace Pan.Affiliation.Domain.Modules.Customers.Queries;

public interface IGetAllCustomersQueryHandler
{
    Task<IEnumerable<Customer>> GetAllCustomersAsync(int skip = 0, int take = 25);
}