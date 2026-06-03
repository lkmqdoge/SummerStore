using Microsoft.AspNetCore.Identity;

using SummerStore.Core.Interfaces;
using SummerStore.Core.Models.Entities;

namespace SummerStore.Core.Services;

internal class UserService(
    IUserRepository userRepository,
    IJwtService jwtService
) : IUserService
{
    public async Task<Guid> RegisterAsync(
        string userName,
        string email,
        string password,
        CancellationToken ct)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = userName,
            Email = email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, password);

        return await userRepository.AddAsync(user, ct);
    }

    public async Task<string> LoginAsync(
        string email,
        string password,
        CancellationToken ct)
    {
        var user = await userRepository.GetByEmailAsync(email)
            ?? throw new Exception("User not found");

        var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Failed
            ? throw new Exception("Passwords not match")
            : jwtService.GenerateToken(user);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken ct)
    {
        return await userRepository.GetAllAsync(ct);
    }
}