using Pan.Affiliation.Domain.Shared;

namespace Pan.Affiliation.Api.Contracts;

public class GenericResponse<T>
{
    public T? Data { get; init; }
    public IEnumerable<Error>? Errors { get; init; } = Enumerable.Empty<Error>();
}