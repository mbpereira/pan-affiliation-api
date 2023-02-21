namespace Pan.Affiliation.Domain.Shared.UseCase
{
    public interface IUseCase<TParam, TResponse>
    {
        Task<TResponse> ExecuteAsync(TParam param);
    }
}
