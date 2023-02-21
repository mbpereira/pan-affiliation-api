
namespace Pan.Affiliation.Domain.Shared.Validation;

public interface IValidationContext
{
    IEnumerable<Error> Errors { get; }
    bool HasErrors { get; }
    ValidationStatus? ValidationStatus { get; }
    void SetStatus(ValidationStatus status);
    void AddNotification(string key, string message);
    void AddNotification(Error error);
    void AddNotifications(IEnumerable<Error> notifications);
    void AddNotifications(ValidationResult validationResult);
}