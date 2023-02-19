namespace Pan.Affiliation.Domain.Settings
{
    public interface ISettingsProvider
    {
        public T GetSection<T>(string key) where T : class, new();
        T? GetValue<T>(string key);
    }
}
