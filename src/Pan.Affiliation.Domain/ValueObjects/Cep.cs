using System.Text.RegularExpressions;

namespace Pan.Affiliation.Domain.ValueObjects
{
    public struct Cep
    {
        public string Value { get; private set; }
        public bool IsValid { get; private set; }

        public Cep(string cep)
        {
            Value = cep;
            IsValid = Validate(cep);
        }

        private static bool Validate(string cep)
            => !string.IsNullOrEmpty(cep) && Regex.IsMatch(cep, Shared.Constants.Regex.ValidCep);

        public static implicit operator Cep(string cep)
            => new(cep);

        public override string ToString()
            => Value;
    }
}
