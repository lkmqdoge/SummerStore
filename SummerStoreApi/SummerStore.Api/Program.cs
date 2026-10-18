using SummerStore.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureApplicationBuilder();

var app = builder.Build();
app.ConfigureApplication();

app.Run();