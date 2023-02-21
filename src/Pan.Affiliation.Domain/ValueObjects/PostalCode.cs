using System.Text.RegularExpressions;
using Pan.Affiliation.Shared.Extensions;
using static Pan.Affiliation.Shared.Constants.Regex;

namespace Pan.Affiliation.Domain.ValueObjects
{
    public struct PostalCode

    {
        public string OriginalValue { get; }
        public string Value { get; }
        public bool IsValid { get; }

        public PostalCode(string cep)
        {
            OriginalValue = cep;
            Value = cep?.OnlyNumbers();
            IsValid = Validate(cep);
        }

        private static bool Validate(string cep)
            => !string.IsNullOrEmpty(cep) && Regex.IsMatch(cep, ValidCep);

        public static implicit operator PostalCode(string cep)
            => new(cep);

        public override string ToString()
            => Value;
    }
}
