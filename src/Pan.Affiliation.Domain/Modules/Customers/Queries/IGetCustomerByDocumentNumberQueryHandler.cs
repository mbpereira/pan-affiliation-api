using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;

namespace Pan.Affiliation.Domain.Modules.Customers.Queries;

public interface IGetCustomerByDocumentNumberQueryHandler
{
    Task<Customer?> GetCustomerByDocumentNumberAsync(DocumentNumber documentNumber);
}