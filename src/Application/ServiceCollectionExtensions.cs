using System.Reflection;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            (serviceProvider, dbContextBuilder) =>
            {
                dbContextBuilder.UseNpgsql(configuration.GetConnectionString("DatabaseConnection"),
                    optionsBuilder =>
                    {
                        optionsBuilder.MigrationsAssembly("Application");
                        optionsBuilder.MigrationsHistoryTable("__EFMigrationsHistory", "public");
                    }).UseSnakeCaseNamingConvention();
            });
        
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        return services;
    }
}
