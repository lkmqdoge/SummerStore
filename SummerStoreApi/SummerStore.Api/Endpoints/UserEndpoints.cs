using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using SummerStore.Core.Interfaces;
using SummerStore.Core.Models.Dto;
using SummerStore.Core.Models.Entities;
using SummerStore.Infrastructure.Data;

namespace SummerStore.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", Register)
            .Produces<AuthResponse>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status409Conflict);

        group.MapPost("/login", Login)
            .Produces<AuthResponse>()
            .Produces(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> Register(
        RegisterRequest req,
        AppDbContext db,
        IPasswordHasher<User> hasher,
        IJwtService tokens,
        CancellationToken ct)
    {
        var errors = new Dictionary<string, string[]>();
        var email = req.Email?.Trim().ToLowerInvariant() ?? "";
        var userName = req.UserName?.Trim() ?? "";

        if (userName.Length is < 3 or > 64)
            errors["userName"] = ["username length should be in range from 3 to 64 symbols"];

        if (!email.Contains('@') || email.Length > 256)
            errors["email"] = ["invalid email"];

        if ((req.Password?.Length ?? 0) < 8)
            errors["password"] = ["password should be at least 8 symbols long"];

        if (errors.Count > 0)
            return Results.ValidationProblem(errors);

        if (await db.Users.AnyAsync(u => u.Email == email || u.UserName == userName, ct))
            return Results.Conflict(new { error = "Пользователь с таким email или именем уже существует" });

        var user = new User
        {
            Id = Guid.CreateVersion7(),
            UserName = userName,
            Email = email
        };

        user.PasswordHash = hasher.HashPassword(user, req.Password!);
        user.Cart = new Cart { Id = Guid.CreateVersion7() };

        db.Users.Add(user);
        await db.SaveChangesAsync(ct);

        return Results.Ok(new AuthResponse(tokens.GenerateToken(user), user.Id, user.UserName));
    }

private static async Task<IResult> Login(
        LoginRequest req,
        AppDbContext db,
        IPasswordHasher<User> hasher,
        IJwtService tokens,
        CancellationToken ct)
    {
        var email = req.Email?.Trim().ToLowerInvariant() ?? "";
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

        return user is null ||
            hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password ?? "") == PasswordVerificationResult.Failed
            ? Results.Unauthorized()
            : Results.Ok(new AuthResponse(tokens.GenerateToken(user), user.Id, user.UserName));
    }
}