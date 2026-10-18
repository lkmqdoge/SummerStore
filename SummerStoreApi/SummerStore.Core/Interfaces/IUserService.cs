using SummerStore.Core.Models.Entities;

namespace SummerStore.Core.Interfaces;
public interface IUserService
{
    Task<Guid> RegisterAsync(
        string userName,
        string email,
        string password,
        CancellationToken cancellationToken = default
    );
    Task<string> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default
    );

    Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
}