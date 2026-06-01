using SummerStore.Core.Models.Entities;

namespace SummerStore.Core.Interfaces;
public interface IJwtService
{
    public string GenerateToken(User user);
}

