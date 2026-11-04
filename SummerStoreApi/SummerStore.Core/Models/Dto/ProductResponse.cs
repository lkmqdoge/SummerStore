namespace SummerStore.Core.Models.Dto;

public sealed record ProductResponse(
    Guid Id,
    Guid CreatorId,
    string Title,
    string Summary,
    string Description,
    decimal Price,
    List<Guid> ImageIds
);
