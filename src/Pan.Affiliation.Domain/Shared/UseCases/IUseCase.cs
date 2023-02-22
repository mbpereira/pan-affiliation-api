namespace Pan.Affiliation.Domain.Shared.UseCases
{
    public interface IUseCase<TParam, TResponse>
    {
        Task<TResponse> ExecuteAsync(TParam param);
    }
}
