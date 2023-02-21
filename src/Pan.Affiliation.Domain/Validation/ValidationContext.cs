using FluentValidation.Results;

namespace Pan.Affiliation.Domain.Shared;

public class ValidationContext : IValidationContext
{
    private readonly List<Error> _errors;
    public IEnumerable<Error> Errors => _errors;
    public bool HasErrors => _errors.Any();
    public ValidationStatus? ValidationStatus { get; private set; }

    public ValidationContext()
    {
        _errors = new List<Error>();
    }

    public void SetStatus(ValidationStatus status) => ValidationStatus = status;

    public void AddNotification(string key, string message)
    {
        _errors.Add(new Error(key, message));
    }

    public void AddNotification(Error error)
    {
        _errors.Add(error);
    }

    public void AddNotifications(IEnumerable<Error> notifications)
    {
        _errors.AddRange(notifications);
    }

    public void AddNotifications(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
        {
            AddNotification(error.ErrorCode, error.ErrorMessage);
        }
    }
}