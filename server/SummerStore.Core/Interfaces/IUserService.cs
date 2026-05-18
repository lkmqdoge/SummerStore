using SummerStore.Core.Models.Entities;

namespace SummerStore.Core.Interfaces;
public interface IUserService
{
    Task<int> AddAsync(
        string userName,
        string password,
        CancellationToken cancellationToken = default
    );

    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
}
