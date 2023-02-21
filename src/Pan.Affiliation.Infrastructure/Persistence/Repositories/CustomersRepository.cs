using Microsoft.EntityFrameworkCore;
using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.Queries;
using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;

namespace Pan.Affiliation.Infrastructure.Persistence.Repositories;

public class CustomersRepository : IGetCustomerByDocumentNumberQuery
{
    private readonly PanAffiliationDbContext _context;

    public CustomersRepository(PanAffiliationDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetCustomerByDocumentNumberAsync(DocumentNumber documentNumber)
    {
        var customer = await _context.Customers!
            .Include(c => c.Addresses)
            .FirstOrDefaultAsync(c => c.DocumentNumber == documentNumber.Value);

        if (customer is null)
            return null;

        return customer.ToDomainEntity();
    }
}