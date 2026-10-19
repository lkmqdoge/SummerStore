namespace SummerStore.Core.Models.Entities;

public sealed class Product
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}