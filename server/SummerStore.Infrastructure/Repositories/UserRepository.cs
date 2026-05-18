using Microsoft.EntityFrameworkCore;

using SummerStore.Core.Interfaces;
using SummerStore.Core.Models.Entities;
using SummerStore.Infrastructure.Data;

namespace SummerStore.Infrastructure.Repositories;

internal class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<int> AddAsync(User user, CancellationToken cancellationToken)
    {
        var created = await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return created.Entity.Id;
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Users.ToListAsync(cancellationToken);
    }
}

