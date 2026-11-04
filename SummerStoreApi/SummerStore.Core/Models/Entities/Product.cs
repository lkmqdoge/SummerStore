namespace SummerStore.Core.Models.Entities;

public sealed class Product : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public Guid CreatorId { get; set; }
    public User Creator { get; set; } = null!;

    public List<Document> Attachments { get; set; } = [];
    public List<CartItem> CartItems { get; set; } = [];
    public List<User> Owners { get; set; } = [];
}