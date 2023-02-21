using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;
using Pan.Affiliation.Domain.Shared.UseCase;

namespace Pan.Affiliation.Domain.Modules.Customers.UseCases;

public interface IGetCustomerByDocumentNumberUseCase : IUseCase<DocumentNumber, Customer?>
{
}