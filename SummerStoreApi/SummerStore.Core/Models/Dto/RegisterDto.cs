namespace SummerStore.Core.Models.Dto;

public record RegisterUserDto(
    string UserName,
    string Email,
    string Password
);
