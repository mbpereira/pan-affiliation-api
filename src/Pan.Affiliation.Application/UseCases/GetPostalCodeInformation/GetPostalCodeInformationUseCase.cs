using Pan.Affiliation.Domain.Localization.Adapters;
using Pan.Affiliation.Domain.Localization.Entities;
using Pan.Affiliation.Domain.Logging;
using Pan.Affiliation.Domain.ValueObjects;

namespace Pan.Affiliation.Application.UseCases.GetPostalCodeInformation;

public class GetPostalCodeInformationUseCase : IGetPostalCodeInformationUseCase
{
    private readonly IPostalCodeInformationService _service;
    private readonly ILogger<GetPostalCodeInformationUseCase> _logger;

    public GetPostalCodeInformationUseCase(IPostalCodeInformationService service,
        ILogger<GetPostalCodeInformationUseCase> logger)
    {
        _service = service;
        _logger = logger;
    }

    public Task<PostalCodeInformation?> ExecuteAsync(PostalCode param)
    {
        _logger.LogInformation("Getting postalCode information {postalCode}", param);
        return _service.GetPostalCodeInformationAsync(param);
    }
}