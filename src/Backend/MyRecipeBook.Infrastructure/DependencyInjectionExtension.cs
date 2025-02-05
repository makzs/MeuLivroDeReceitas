using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void addInfrastructure(this IServiceCollection services)
    {
        addDbContext(services);
        addRepositories(services);
    }

    private static void addDbContext(IServiceCollection services)
    {
        var connectionString = "Data Source=SQLEXPRESS;Initial Catalog=meulivrodereceitas;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;";
        services.AddDbContext<MyRecipeBookDbContext>(dbContextOptions =>
        {
            dbContextOptions.UseSqlServer(connectionString);

        });
    }
    private static void addRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
    }
}
