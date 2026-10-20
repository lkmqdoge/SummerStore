namespace SummerStore.Core.Models.Entities;

public class CartItem
{
    public Guid Id { get; set; }
    public Guid Productid { get; set; }
    public Guid CartId { get; set; }

    public int Quantity { get; set; }

    public Product Product { get; set; } = null!;
    public Cart Cart { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

