using Microsoft.EntityFrameworkCore;

using SummerStore.Infrastructure.Data;

namespace SummerStore.Api.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention());

        return services;
    }
}
