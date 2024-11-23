using Schnauz.CommandHandlers.Core;
using Schnauz.QueryHandlers.Core;
using Schnauz.Shared.Dtos;
using Schnauz.Shared.Interfaces;
using Schnauz.Shared;

namespace Schnauz.Server;

public static class Setup
{
    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddTransient<IQueryExecutor, QueryExecuter>();
        services.AddTransient<IQueryExecutorServer, QueryExecuter>();
        services.AddTransient<ICommandExecutor, CommandExecuter>();

        foreach (var (iFace, implementation) in InterfaceMappingHelper.MappingByConventionBasedOnInterface(typeof(IQueryHandlerQuery<>)))
        {
            services.AddTransient(iFace, implementation);
        }
        foreach (var (iFace, implementation) in InterfaceMappingHelper.MappingByConventionBasedOnInterface(typeof(ICommandHandler<>)))
        {
            services.AddTransient(iFace, implementation);
        }

        return services;
    }

    public static IServiceCollection AddValidations(this IServiceCollection services)
    {
        return services.AddFluentValidations([typeof(ValidationErrorDto)]);
    }
}
