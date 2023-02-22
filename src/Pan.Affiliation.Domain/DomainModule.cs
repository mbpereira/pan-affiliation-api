using Autofac;

namespace Pan.Affiliation.Domain;

public class DomainModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(DomainModule).Assembly)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}