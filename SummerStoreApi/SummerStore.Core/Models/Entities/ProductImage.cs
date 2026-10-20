namespace SummerStore.Core.Models.Entities;

public sealed class ProductImage
{
    public Guid ProductId { get; set; }

    public Product Product { get; set; } = null!;
    public required byte[] Image { get; set; }
}