namespace Pan.Affiliation.Domain.Shared;

public class Error
{
    public string Key { get; }
    public string Message { get; }
    
    public Error(string message, string key)
    {
        Message = message;
        Key = key;
    }
}