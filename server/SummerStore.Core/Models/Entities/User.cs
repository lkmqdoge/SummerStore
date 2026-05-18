using System.ComponentModel.DataAnnotations;

namespace SummerStore.Core.Models.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

}