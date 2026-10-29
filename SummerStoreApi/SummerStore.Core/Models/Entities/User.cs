namespace SummerStore.Core.Models.Entities;

public sealed class User : BaseEntity
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public Cart? Cart { get; set; }
}