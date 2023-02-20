using FluentAssertions;
using Pan.Affiliation.Domain.ValueObjects;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Domain.ValueObjects
{
    public class PostalCodeTests
    {
        [Theory]
        [InlineData("78085630")]
        [InlineData("78085-630")]
        public void Cep_IsValid_ShouldReturnTrueIfCepIsValid(string validPostalCode)
        {
            PostalCode postalCode = new(validPostalCode);

            var isValid = postalCode.IsValid;

            isValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("12345")]
        [InlineData("12345-")]
        [InlineData("1234567")]
        [InlineData("1234567-")]
        [InlineData(null)]
        [InlineData("12345678911")]
        [InlineData("123456789")]
        [InlineData("123456-789")]
        [InlineData("12345-7890")]
        public void Cep_IsValid_ShouldReturnFalseIfCepIsNotValid(string invalidPostalCode)
        {
            PostalCode postalCode = new(invalidPostalCode);

            var isValid = postalCode.IsValid;

            isValid.Should().BeFalse();
        }
    }
}
