using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Shared.UseCases;

namespace Pan.Affiliation.Domain.Modules.Customers.UseCases.GetAllCustomers;

public interface IGetAllCustomersUseCase : IUseCase<int, IEnumerable<Customer>?>
{
}