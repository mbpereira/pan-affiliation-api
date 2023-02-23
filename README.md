## Adicionando migrations
dotnet ef migrations add FixDateTimeType --startup-project ./src/Pan.Affiliation.Api --project ./src/Pan.Affiliation.Infrastructure --output-dir Persistence/Migrations
## Aplicando migrations
dotnet ef database update --startup-project ./src/Pan.Affiliation.Api --project ./src/Pan.Affiliation.Infrastructure
## Executando a aplicação localmente
- sudo docker-compose build .
- sudo docker-compose up -d --no-recreate
