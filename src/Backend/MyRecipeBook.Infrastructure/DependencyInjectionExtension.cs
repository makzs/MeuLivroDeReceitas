using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void addInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        addDbContext(services, configuration);
        addRepositories(services);
    }

    private static void addDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
        {
            var connectionString = configuration.GetConnectionString("Connection");

            dbContextOptions.UseSqlServer(connectionString);

        });
    }
    private static void addRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnityOfWork, UnityOfWork>();
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }
}
