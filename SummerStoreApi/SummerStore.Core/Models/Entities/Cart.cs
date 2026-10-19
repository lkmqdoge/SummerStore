namespace SummerStore.Core.Models.Entities;

public sealed class Cart
{
    public required Guid Id { get; set; }

    public required Guid UserId { get; set; }

    public User User { get; set; } = null!;

    public ICollection<Product> Products { get; set; } = [];
}
