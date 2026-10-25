namespace SummerStore.Core.Models.Dto;

public sealed record AuthResponse(
    string Token,
    Guid UserId,
    string UserName
);

