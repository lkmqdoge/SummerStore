namespace SummerStore.Core.Models.Dto;

public sealed record ProductResponse(
    Guid Id,
    string Title,
    string Summary,
    string Description,
    decimal Price,
    List<Guid> ImageIds
);

