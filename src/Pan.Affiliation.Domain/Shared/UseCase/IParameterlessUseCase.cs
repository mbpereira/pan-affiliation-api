namespace Pan.Affiliation.Domain.Shared.UseCase
{
    public interface IParameterlessUseCase<TResponse>
    {
        Task<TResponse> ExecuteAsync();
    }
}
