using Pan.Affiliation.Domain.Shared;

namespace Pan.Affiliation.Api.Contracts;

public class GenericResponse<T>
{
    public T Data { get; set; }
    public IEnumerable<Error>? Errors { get; set; } = Enumerable.Empty<Error>();
}