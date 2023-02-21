using FluentAssertions;
using Pan.Affiliation.Domain.Modules.Merchant;
using Pan.Affiliation.Domain.Modules.Merchant.Enums;
using Pan.Affiliation.Domain.Modules.Merchant.ValueObjects;
using Pan.Affiliation.Shared.Extensions;

namespace Pan.Affiliation.UnitTests.Pan.Affiliation.Domain.Modules.Merchant.ValueObjects;

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
        var documentNumberVO = new DocumentNumber(document);

        var isValid = documentNumberVO.IsValid();

        isValid.Should().BeTrue();
        documentNumberVO.DocumentType.Should().Be(expectedDocumentType);
        documentNumberVO.OriginalValue.Should().Be(document);
        documentNumberVO.Value.Should().Be(document.OnlyNumbers());
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
        var documentNumberVO = new DocumentNumber(document);

        var isValid = documentNumberVO.IsValid();

        isValid.Should().BeFalse();
        documentNumberVO.DocumentType.Should().BeNull();
    }
}