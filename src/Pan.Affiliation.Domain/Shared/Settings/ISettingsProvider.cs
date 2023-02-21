namespace Pan.Affiliation.Domain.Shared.Settings
{
    public interface ISettingsProvider
    {
        public T GetSection<T>(string key) where T : class, new();
        T? GetValue<T>(string key);
    }
}
