namespace Pan.Affiliation.Domain.Shared.UseCases
{
    public interface IParameterlessUseCase<TResponse>
    {
        Task<TResponse> ExecuteAsync();
    }
}
