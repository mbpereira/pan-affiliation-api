namespace Pan.Affiliation.Shared.Extensions
{
    public static class EnumerableExtensions
    {
        public static bool Includes(this IEnumerable<string> strs, string value)
            => strs.Any(str => value.Equals(str, StringComparison.InvariantCultureIgnoreCase));
    }
}
