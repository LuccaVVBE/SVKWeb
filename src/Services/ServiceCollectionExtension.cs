using Microsoft.Extensions.DependencyInjection;
using Svk.Services.Controle;
using Svk.Services.File;
using Svk.Services.User;
using Svk.Shared.Controles;
using Svk.Shared.Files;
using Svk.Shared.Users;

namespace Svk.Services;

public static class ServiceCollectionExtension
{
    public static IServiceCollection addSvkServices(this IServiceCollection services)
    {
        services.AddScoped<IControleService, ControleService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ILoaderService, LoaderService>();
        services.AddScoped<IManagerService, ManagerService>();
        return services;
    }
}