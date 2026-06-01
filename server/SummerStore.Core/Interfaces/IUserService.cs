using SummerStore.Core.Models.Entities;

namespace SummerStore.Core.Interfaces;
public interface IUserService
{
    Task<Guid> AddAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default
    );

    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);

    // Task<User?> GetByIdAsync(int id, CancellationToken ct);
}