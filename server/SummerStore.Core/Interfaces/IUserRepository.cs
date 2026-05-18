using SummerStore.Core.Models.Entities;

namespace SummerStore.Core.Interfaces;

public interface IUserRepository
{
    Task<int> AddAsync(User user, CancellationToken cancellationToken);
    Task<List<User>> GetAllAsync(CancellationToken cancellationToken);
}
