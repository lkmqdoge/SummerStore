using Microsoft.EntityFrameworkCore;

using SummerStore.Core.Models.Entities;

namespace SummerStore.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}