namespace Pan.Affiliation.Application.UseCases
{
    public interface IUseCase<TParam, TResponse>
    {
        Task<TResponse> Execute(TParam param);
    }
}
