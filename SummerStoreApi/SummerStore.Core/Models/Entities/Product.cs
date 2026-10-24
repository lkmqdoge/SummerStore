namespace SummerStore.Core.Models.Entities;

public sealed class Product : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public List<ProductImage> Images { get; set; } = [];
    public List<CartItem> CartItems { get; set; } = [];
}