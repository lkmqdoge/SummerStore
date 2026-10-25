namespace SummerStore.Core.Models.Dto;

public sealed record CartItemResponse(
    Guid ProductId,
    string Title,
    decimal Price,
    int Quantity,
    decimal LineTotal
);
