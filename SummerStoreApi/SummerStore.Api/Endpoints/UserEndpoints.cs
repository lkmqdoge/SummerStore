using SummerStore.Core.Interfaces;
using SummerStore.Core.Models.Dto;

namespace SummerStore.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app
            .MapGroup("/api/");

        group.MapPost("register", Register);
        group.MapPost("login", Login);
    }

    public static async Task<IResult> Login(
        LoginUserDto request,
        IUserService userService,
        CancellationToken ct)
    {
        var token = await userService.LoginAsync(request.Email, request.Password, ct);
        return Results.Ok(token);
    }
    public static async Task<IResult> Register(
        RegisterUserDto request,
        IUserService userService,
        CancellationToken ct)
    {
        var created = await userService.RegisterAsync(
            request.UserName,
            request.Email,
            request.Password, ct);
        return Results.Created($"/api/users/{created}", created);
    }

    // public static async Task<IResult> Create(
    //         string username,
    //         string password,
    //         IUserService userService,
    //         CancellationToken ct)
    // {
    //     var created = await userService.AddAsync(username, password, ct);
    //     return Results.Created($"/api/users/{created}", created);
    // }

    public static async Task<IResult> GetAll(IUserService userService, CancellationToken ct)
    {
        return Results.Json(await userService.GetAllAsync(ct));
    }
}