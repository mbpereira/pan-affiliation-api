using Pan.Affiliation.Domain.Modules.Localization.Entities;
using Pan.Affiliation.Domain.Modules.Localization.Gateways;
using Pan.Affiliation.Domain.Shared.Logging;
using Pan.Affiliation.Domain.Shared.ValueObjects;

namespace Pan.Affiliation.Application.UseCases.GetPostalCodeInformation;

public class GetPostalCodeInformationUseCase : IGetPostalCodeInformationUseCase
{
    private readonly IPostalCodeInformationGatewayService _gatewayService;
    private readonly ILogger<GetPostalCodeInformationUseCase> _logger;

    public GetPostalCodeInformationUseCase(IPostalCodeInformationGatewayService gatewayService,
        ILogger<GetPostalCodeInformationUseCase> logger)
    {
        _gatewayService = gatewayService;
        _logger = logger;
    }

    public Task<PostalCodeInformation?> ExecuteAsync(PostalCode param)
    {
        _logger.LogInformation("Getting postalCode information {postalCode}", param);
        return _gatewayService.GetPostalCodeInformationAsync(param);
    }
}