using SummerStore.Core.Models.Entities;

namespace SummerStore.Core.Interfaces;

public interface IUserRepository
{
    Task<Guid> AddAsync(User user, CancellationToken cancellationToken);

    Task<User?> GetByEmailAsync(string email);

    Task<List<User>> GetAllAsync(CancellationToken cancellationToken);

    // Task<User?> GetByIdAsync(int id, CancellationToken ct);
}