namespace SummerStore.Core.Models.Dto;

public sealed record AddCartItemRequest(
    Guid ProductId,
    int Quantity
);