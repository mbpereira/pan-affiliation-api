using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Shared.UseCases;

namespace Pan.Affiliation.Domain.Modules.Customers.UseCases.CreateCustomer;

public interface ICreateCustomerUseCase : IUseCase<CreateCustomerInput, Customer?>
{
}