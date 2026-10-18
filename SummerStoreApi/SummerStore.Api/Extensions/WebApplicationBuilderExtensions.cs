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

        #region Project Dependencies

        builder.Services
            .AddDataAccess(builder.Configuration)
            .AddCore(builder.Configuration);

        #endregion Project Dependencies

        builder.Services
            .AddAuthorization();

        return builder;
    }
}