using SummerStore.Api.Endpoints;

namespace SummerStore.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product service v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.MapUserEndpoints();
        app.MapProductEndpoints();
        app.MapCartEndpoints();
        app.UseCors("Frontend");
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}