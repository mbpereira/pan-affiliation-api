using Pan.Affiliation.Domain.Modules.Customers.Commands;
using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.Queries;
using Pan.Affiliation.Domain.Modules.Customers.UseCases.CreateCustomer;
using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Application.UseCases.Customers;

public class CreateCustomerUseCase : ICreateCustomerUseCase
{
    private readonly IValidationContext _context;
    private readonly ICreateCustomerCommandHandler _command;
    private readonly ILogger<CreateCustomerUseCase> _logger;
    private readonly IGetCustomerByDocumentNumberQueryHandler _query;

    public CreateCustomerUseCase(
        ILogger<CreateCustomerUseCase> logger,
        ICreateCustomerCommandHandler command,
        IValidationContext context,
        IGetCustomerByDocumentNumberQueryHandler query)
    {
        _logger = logger;
        _command = command;
        _context = context;
        _query = query;
    }

    public async Task<Customer?> ExecuteAsync(CreateCustomerInput param)
    {
        var customer = param.ToDomainEntity();

        var validationResult = customer.Validate();

        if (!validationResult.IsValid)
        {
            _context.AddNotifications(validationResult.Errors);
            _logger.LogInformation("Invalid customer data");

            return null;
        }

        var existingCustomer = await _query.GetCustomerByDocumentNumberAsync(customer.DocumentNumber);

        if (existingCustomer is not null)
        {
            _context.SetStatus(ValidationStatus.Conflict);
            _context.AddNotification(
                nameof(Shared.Constants.Errors.ConflictError),
                Shared.Constants.Errors.ConflictError);
            _logger.LogInformation("Provided document already exists");

            return null;
        }

        return await _command.CreateCustomerAsync(customer);
    }
}