using FluentAssertions;
using Pan.Affiliation.Domain.Modules.Customers.Enums;
using Pan.Affiliation.Domain.Modules.Customers.ValueObjects;
using Pan.Affiliation.Shared.Extensions;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Domain.Modules.Customers.ValueObjects;

public class DocumentNumberTests
{
    [Theory]
    [InlineData("05915193137", DocumentType.Cpf)]
    [InlineData("059.151.931-37", DocumentType.Cpf)]
    [InlineData("13984572000102", DocumentType.Cnpj)]
    [InlineData("96.273.366/0001-03", DocumentType.Cnpj)]
    public void Should_return_true_and_set_document_type_when_document_number_is_valid(string document,
        DocumentType expectedDocumentType)
    {
        var documentNumberVo = new DocumentNumber(document);

        var isValid = documentNumberVo.IsValid();

        isValid.Should().BeTrue();
        documentNumberVo.DocumentType.Should().Be(expectedDocumentType);
        documentNumberVo.OriginalValue.Should().Be(document);
        documentNumberVo.Value.Should().Be(document.OnlyNumbers());
    }
    
    [Theory]
    [InlineData("0591519313")]
    [InlineData("059.151.931-3")]
    [InlineData("1398457200010")]
    [InlineData("96.273.366/0001-3")]
    [InlineData("00000000000000")]
    [InlineData("00000000000")]
    [InlineData(null)]
    [InlineData("")]
    public void Should_return_false_when_document_number_is_not_valid(string document)
    {
        var documentNumberVo = new DocumentNumber(document);

        var isValid = documentNumberVo.IsValid();

        isValid.Should().BeFalse();
        documentNumberVo.DocumentType.Should().BeNull();
    }
}