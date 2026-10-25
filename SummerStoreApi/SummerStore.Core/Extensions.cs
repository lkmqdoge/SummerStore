using System.Text;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using SummerStore.Core.Interfaces;
using SummerStore.Core.Models.Entities;
using SummerStore.Core.Services;

namespace SummerStore.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authSettings = configuration.GetSection(nameof(AuthSettings))
            .Get<AuthSettings>()
            ?? throw new InvalidOperationException($"Required configuration section \"{nameof(AuthSettings)}\" is missing");


        services.AddScoped<IUserService, UserService>()
            .AddScoped<IJwtService, JwtService>()
            .Configure<AuthSettings>(configuration.GetSection("AuthSettings"))
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o => o.TokenValidationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.SecretKey))
            });

        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

        return services;
    }
}