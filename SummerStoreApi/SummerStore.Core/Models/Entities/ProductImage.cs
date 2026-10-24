namespace SummerStore.Core.Models.Entities;

public sealed class ProductImage
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public required byte[] Image { get; set; }
    public string ContentType { get; set; } = "image/jpeg";
    public int SortOrder { get; set; }
}