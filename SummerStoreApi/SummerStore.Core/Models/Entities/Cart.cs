namespace SummerStore.Core.Models.Entities;

public sealed class Cart : BaseEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public List<CartItem> Items { get; set; } = [];
}