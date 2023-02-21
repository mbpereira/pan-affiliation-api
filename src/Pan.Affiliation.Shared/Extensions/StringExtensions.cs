using System.Text.RegularExpressions;

namespace Pan.Affiliation.Shared.Extensions;

public static class StringExtensions
{
    public static string? OnlyNumbers(this string str)
    {
        if (string.IsNullOrEmpty(str))
            return null;
        
        return Regex.Replace(str, "[^0-9]", "");
    }
}