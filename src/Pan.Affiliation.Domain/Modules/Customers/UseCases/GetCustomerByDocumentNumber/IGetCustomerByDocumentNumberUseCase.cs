using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;
using Pan.Affiliation.Domain.Shared.UseCases;

namespace Pan.Affiliation.Domain.Modules.Customers.UseCases.GetCustomerByDocumentNumber;

public interface IGetCustomerByDocumentNumberUseCase : IUseCase<DocumentNumber, Customer?>
{
}