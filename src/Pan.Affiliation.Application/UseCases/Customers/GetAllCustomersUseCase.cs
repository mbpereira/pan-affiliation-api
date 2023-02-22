using Pan.Affiliation.Domain.Modules.Customers.Entities;
using Pan.Affiliation.Domain.Modules.Customers.Queries;
using Pan.Affiliation.Domain.Modules.Customers.UseCases.GetAllCustomersUseCase;
using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.Validation;

namespace Pan.Affiliation.Application.UseCases.Customers;

public class GetAllCustomersUseCase : IGetAllCustomersUseCase
{
    private readonly IGetAllCustomersQueryHandler _query;
    private readonly IValidationContext _validationContext;
    private readonly ILogger<GetAllCustomersUseCase> _logger;

    public GetAllCustomersUseCase(
        IGetAllCustomersQueryHandler query,
        IValidationContext validationContext, 
        ILogger<GetAllCustomersUseCase> logger)
    {
        _query = query;
        _validationContext = validationContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Customer>?> ExecuteAsync(int param)
    {
        if (param <= 0)
        {
            _validationContext.AddNotification(
                nameof(Shared.Constants.Errors.InvalidPageError),
                Shared.Constants.Errors.InvalidPageError);
            _logger.LogInformation("Page {page} is not valid", param);
            return null;
        }

        var take = 25;
        var skip = ((param - 1) * take);

        return await _query.GetAllCustomersAsync(skip, take);
    }
}