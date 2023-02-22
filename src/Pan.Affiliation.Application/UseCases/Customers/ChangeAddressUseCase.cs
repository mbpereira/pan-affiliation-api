using Pan.Affiliation.Domain.Modules.Customers.Commands;
using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.Queries;
using Pan.Affiliation.Domain.Modules.Customers.UseCases.ChangeAddress;
using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.Validation;
using Address = Pan.Affiliation.Domain.Modules.Customers.Entities.Address;

namespace Pan.Affiliation.Application.UseCases.Customers;

public class ChangeAddressUseCase : IChangeAddressUseCase
{
    private readonly IChangeCustomerCommandHandler _command;
    private readonly IGetCustomerByIdQueryHandler _query;
    private readonly IValidationContext _context;
    private readonly ILogger<ChangeAddressUseCase> _logger;

    public ChangeAddressUseCase(
        IChangeCustomerCommandHandler command,
        IGetCustomerByIdQueryHandler query,
        ILogger<ChangeAddressUseCase> logger,
        IValidationContext context)
    {
        _command = command;
        _query = query;
        _logger = logger;
        _context = context;
    }

    public async Task<Address?> ExecuteAsync(ChangeAddressInput param)
    {
        var logScope = new Dictionary<string, object>
        {
            { "CustomerId", param.CustomerId },
            { "AddressId", param.AddressId }
        };
        
        using var _ = _logger.BeginScope(logScope);
        
        var customer = await _query.GetCustomerByIdAsync(param.CustomerId);

        if (customer is null)
        {
            _logger.LogWarning("Customer was not found");
            _context.SetStatus(ValidationStatus.NotFound);
            _context.AddNotification(new Error(
                nameof(Customer),
                Shared.Constants.Errors.RecordNotFound));
            return null;
        }

        if (customer.Addresses.All(a => a.Id != param.AddressId))
        {
            _logger.LogWarning("Address was not found");
            _context.SetStatus(ValidationStatus.NotFound);
            _context.AddNotification(new Error(
                nameof(Address),
                Shared.Constants.Errors.RecordNotFound));

            return null;
        }

        var @new = param.ToDomainAddress();

        customer.ChangeAddress(@new);

        var validationResult = customer.Validate();

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Provided address is not valid: {errors}", validationResult.Errors);
            _context.AddNotifications(validationResult.Errors);
            return null;
        }

        await _command.ChangeCustomerAsync(customer);

        return @new;
    }
}