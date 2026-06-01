using Microsoft.AspNetCore.Identity;

using SummerStore.Core.Interfaces;
using SummerStore.Core.Models.Entities;

namespace SummerStore.Core.Services;

internal class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<Guid> AddAsync(string username, string password, CancellationToken ct)
    {
        var now = DateTime.Now.ToUniversalTime();
        var user = new User{
            Id = Guid.NewGuid(),
            UserName = username,
            CreatedAt = now,
            UpdatedAt = now,
        };
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, password);

        return await userRepository.AddAsync(user, ct);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken ct)
    {
        return await userRepository.GetAllAsync(ct);
    }

    // public async Task<User?> GetByIdAsync(int id, CancellationToken ct)
    // {
    //     return await userRepository.GetByIdAsync(id, ct);
    // }
}