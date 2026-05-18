using Microsoft.Extensions.DependencyInjection;

using SummerStore.Core.Interfaces;
using SummerStore.Core.Services;

namespace SummerStore.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}


