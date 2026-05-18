using Microsoft.OpenApi;

using SummerStore.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Product service v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.Run();