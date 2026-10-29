using Microsoft.OpenApi;

using SummerStore.Core;
using SummerStore.Infrastructure;

namespace SummerStore.Api.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder ConfigureApplicationBuilder(this WebApplicationBuilder builder)
    {

        #region Swagger

        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Product service",
            Version = "v1",
            Description = "Project for study",
            Contact = new OpenApiContact
            {
                Name = "lkmqdoge",
                Email = "lkmqdoge@proton.me"
            }
        }));

        #endregion Swagger

        builder.Services.AddCors(options => options.AddPolicy("Frontend", policy => policy
            .WithOrigins(
                "http://frontend:3000",
                "http://localhost:3000",
                "http://localhost:5173",
                "http://localhost:5174"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
        ));

        #region Project Dependencies

        builder.Services.AddDataAccess(builder.Configuration)
            .AddCore(builder.Configuration);

        #endregion Project Dependencies

        builder.Services.AddAuthorization();
        _ = builder.Services.AddTransient(p =>
        {
            var loggerFactory = p.GetRequiredService<ILoggerFactory>();
            return loggerFactory.CreateLogger("My logger");
        });

        return builder;
    }
}