using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Modules.Localization.Queries;
using Pan.Affiliation.Domain.Modules.Localization.UseCases;
using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.ValueObjects;

namespace Pan.Affiliation.Application.UseCases.Localization;

public class GetPostalCodeInformationUseCase : IGetPostalCodeInformationUseCase
{
    private readonly IGetPostalCodeInformationQuery _query;
    private readonly ILogger<GetPostalCodeInformationUseCase> _logger;

    public GetPostalCodeInformationUseCase(IGetPostalCodeInformationQuery query,
        ILogger<GetPostalCodeInformationUseCase> logger)
    {
        _query = query;
        _logger = logger;
    }

    public Task<PostalCodeInformation?> ExecuteAsync(PostalCode param)
    {
        _logger.LogInformation("Getting postalCode information {postalCode}", param);
        return _query.GetPostalCodeInformationAsync(param);
    }
}