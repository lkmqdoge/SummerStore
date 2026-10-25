namespace SummerStore.Core.Models.Dto;

public sealed record ProductListItem(
    Guid Id,
    string Title,
    string Summary,
    decimal Price,
    Guid? PreviewImageId
);