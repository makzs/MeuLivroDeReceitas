using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Application.UseCases.User.Register;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
    public static void addApplication(this IServiceCollection services, IConfiguration configuration)
    {
         addUseCases(services);
        addAutoMapper(services);
        addPasswordEncrypter(services, configuration);
    }

    public static void addAutoMapper(IServiceCollection services)
    {
        services.AddScoped(options => new AutoMapper.MapperConfiguration(options => {
            options.AddProfile(new Automapping());
        }).CreateMapper());
    }

    public static void addUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    }
    public static void addPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
    {
        var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");

        services.AddScoped(option => new PasswordEncripter(additionalKey!));
    }
}
