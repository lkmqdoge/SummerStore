using SummerStore.Core.Interfaces;
using SummerStore.Core.Models.Entities;

namespace SummerStore.Core.Services;

internal class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<int> AddAsync(string userName, string password, CancellationToken cancellationToken)
    {
        var user = new User{
            UserName = userName,
            Password = password,
        };

        return await userRepository.AddAsync(user, cancellationToken);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await userRepository.GetAllAsync(cancellationToken);
    }
}

