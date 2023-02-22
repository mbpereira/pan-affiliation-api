using Microsoft.EntityFrameworkCore;
using Pan.Affiliation.Domain.Modules.Customers.Commands;
using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.Queries;
using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;
using Address = Pan.Affiliation.Infrastructure.Persistence.Entities.Address;

namespace Pan.Affiliation.Infrastructure.Persistence.Repositories;

public class CustomersRepository : 
    IGetCustomerByDocumentNumberQueryHandler, 
    IChangeCustomerCommandHandler,
    IGetCustomerByIdQueryHandler
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

    public async Task<Customer?> ChangeCustomerAsync(Customer customer)
    {
        var old = await GetCustomerEntityByIdAsync(customer.Id);

        if (old is null)
            return null;

        var @new = await ChangeCustomerAsync(old, Entities.Customer.FromDomainEntity(customer));

        return @new.ToDomainEntity();
    }

    public async Task<Customer?> GetCustomerByIdAsync(Guid id)
    {
        var customer = await GetCustomerEntityByIdAsync(id);

        if (customer is null)
            return null;

        return customer.ToDomainEntity();
    }
        
    public async Task<Entities.Customer?> GetCustomerEntityByIdAsync(Guid id)
        => await _context
                .Customers!
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == id);

    private async Task<Persistence.Entities.Customer> ChangeCustomerAsync(Persistence.Entities.Customer old,
        Persistence.Entities.Customer @new)
    {
        _context.Entry(old).CurrentValues.SetValues(@new);

        await ChangeAddressesAsync(
            old.Addresses, 
            @new.Addresses);

        await _context.SaveChangesAsync();

        return @new;
    }

    private async Task ChangeAddressesAsync(IEnumerable<Address> oldAddresses, IEnumerable<Address>? newAddresses)
    {
        foreach (var old in oldAddresses)
        {
            var @new = newAddresses?.FirstOrDefault(a => a.Id == old.Id);

            if (@new is null) continue;

            _context.Entry(old).CurrentValues.SetValues(@new);
        }

        var addressesToRemove = oldAddresses
            .Where(old => newAddresses.All(@new => old.Id != @new.Id));
        var addressesToInsert = newAddresses
            .Where(@new => oldAddresses.All(old => old.Id != @new.Id));

        if (addressesToRemove.Any())
            _context.Addresses!.RemoveRange(addressesToRemove);

        await _context.Addresses!.AddRangeAsync(addressesToInsert);
    }
}