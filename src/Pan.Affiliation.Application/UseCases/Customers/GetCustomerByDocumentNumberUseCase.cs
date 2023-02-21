using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.Queries;
using Pan.Affiliation.Domain.Modules.Customers.UseCases;
using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;
using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Application.UseCases.Customers;

public class GetCustomerByDocumentNumberUseCase : IGetCustomerByDocumentNumberUseCase
{
    private readonly IGetCustomerByDocumentNumberQuery _query;
    private readonly ILogger<GetCustomerByDocumentNumberUseCase> _logger;
    private readonly IValidationContext _validationContext;

    public GetCustomerByDocumentNumberUseCase(IGetCustomerByDocumentNumberQuery query,
        ILogger<GetCustomerByDocumentNumberUseCase> logger, IValidationContext validationContext)
    {
        _query = query;
        _logger = logger;
        _validationContext = validationContext;
    }

    public async Task<Customer?> ExecuteAsync(DocumentNumber param)
    {
        var logScope = new Dictionary<string, object> { { "DocumentNumber", param.ToString() } };

        using var _ = _logger.BeginScope(logScope);

        _logger.LogInformation("Searching customer by document number");

        if (!param.IsValid())
        {
            _logger.LogWarning("Provided document number is not valid");
            _validationContext.AddNotification(nameof(DocumentNumber),
                Shared.Constants.Errors.InvalidDocumentNumberErrorMessage);
            
            return null;
        }

        var foundCustomer = await _query.GetCustomerByDocumentNumberAsync(param);

        if (foundCustomer is null)
        {
            _logger.LogInformation("Customer was not found");
            _validationContext.SetStatus(ValidationStatus.NotFound);
            
            return null;
        }

        _logger.LogInformation("Customer found");
        
        return foundCustomer;
    }
}