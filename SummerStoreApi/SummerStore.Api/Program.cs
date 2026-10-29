using Microsoft.EntityFrameworkCore;

using SummerStore.Api.Extensions;
using SummerStore.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureApplicationBuilder();

var app = builder.Build();
app.ConfigureApplication();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error while trying to apply migrations: {ex.Message}");
        throw;
    }
}

app.Run();