namespace SummerStore.Core.Models.Dto;

public sealed record RegisterRequest(
    string UserName,
    string Email,
    string Password
);