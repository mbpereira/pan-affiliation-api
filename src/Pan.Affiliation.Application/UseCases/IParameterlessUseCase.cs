namespace Pan.Affiliation.Application.UseCases
{
    public interface IParameterlessUseCase<TResponse>
    {
        Task<TResponse> Execute();
    }
}
