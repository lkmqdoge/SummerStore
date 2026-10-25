namespace SummerStore.Core.Models.Dto;

public sealed record ProductRequest(
    string Title,
    string Summary,
    string Description,
    decimal Price
);

