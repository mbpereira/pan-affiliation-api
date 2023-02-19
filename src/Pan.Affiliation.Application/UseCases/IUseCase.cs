namespace Pan.Affiliation.Application.UseCases
{
    public interface IUseCase<TParam, TResponse>
    {
        Task<TResponse> ExecuteAsync(TParam param);
    }
}
