using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Pan.Affiliation.Infrastructure.Data;

namespace Pan.Affiliation.Infrastructure
{
    public class InfrastructureModule : Module
    {
        private readonly IConfiguration _configuration;

        public InfrastructureModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
            services.AddDbContext<PanAffiliationDbContext>(builder =>
                builder.UseNpgsql(GetConnectionString(), 
                    b => b.MigrationsAssembly(GetMigrationsAssembly())));
        }

        private static string? GetMigrationsAssembly()
            => typeof(PanAffiliationDbContext).Assembly.FullName;

        private string GetConnectionString()
        {
            return ""; 
        }
    }
}
