namespace SummerStore.Core.Models.Dto;

public sealed record LoginRequest(
    string Email,
    string Password
);

