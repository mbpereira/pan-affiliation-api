## Adicionando migrations
dotnet ef migrations add AddedCustomerAndAddressTable --startup-project .\src\Pan.Affiliation.Api --project .\src\Pan.Affiliation.Infrastructure\ --output-dir Data\Migrations

## Aplicando migrations
dotnet ef database update --startup-project .\src\Pan.Affiliation.Api --project .\src\Pan.Affiliation.Infrastructure
