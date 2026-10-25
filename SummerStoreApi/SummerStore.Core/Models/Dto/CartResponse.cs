namespace SummerStore.Core.Models.Dto;

public sealed record CartResponse(
    Guid CartId,
    List<CartItemResponse> Items,
    decimal Total
);