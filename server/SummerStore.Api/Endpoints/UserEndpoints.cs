using SummerStore.Core.Interfaces;

namespace SummerStore.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app
            .MapGroup("/api/users");

        group.MapGet("/", GetAll);
        group.MapPost("/", Create);
    }

    public static async Task<IResult> Create(IUserService userService, CancellationToken ct)
    {
        var created = await userService.AddAsync("TEST", "test", ct);
        return Results.Created($"/api/users/{created}", created);
    }
    public static async Task<IResult> GetAll(IUserService userService, CancellationToken ct)
    {
        return Results.Ok(await userService.GetAllAsync(ct));
    }
}

