using SummerStore.Core.Interfaces;

namespace SummerStore.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app
            .MapGroup("/api/users");

        group.MapGet("/", GetAll);
        // group.MapGet("/{id:int}", GetById);
        group.MapPost("/", Create);
    }

    public static async Task<IResult> Create(
            string username,
            string password,
            IUserService userService,
            CancellationToken ct)
    {
        var created = await userService.AddAsync(username, password, ct);
        return Results.Created($"/api/users/{created}", created);
    }

    // public static async Task<IResult> GetById(int id, IUserService userService, CancellationToken ct)
    // {
    //     var user = await userService.GetByIdAsync(id, ct);
    //     return user == null ? Results.NotFound() : Results.Json(await userService.GetByIdAsync(id, ct));
    // }
    public static async Task<IResult> GetAll(IUserService userService, CancellationToken ct)
    {
        return Results.Json(await userService.GetAllAsync(ct));
    }
}