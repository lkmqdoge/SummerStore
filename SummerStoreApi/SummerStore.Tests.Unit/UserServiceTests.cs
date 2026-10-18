using Moq;
using SummerStore.Core.Interfaces;
using SummerStore.Core.Services;

namespace SummerStore.Tests.Unit;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repoMock = new();

    private readonly Mock<IJwtService> _jwtMock = new();
        
    private readonly UserService _userService;

    UserServiceTests()
    {
        _userService = new(_repoMock.Object, _jwtMock.Object);
    }

    [Fact]
    public async Task CreateUser_TwoWithSameEmail()
    {
    }
}