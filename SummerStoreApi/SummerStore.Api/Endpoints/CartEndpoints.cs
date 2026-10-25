using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

using SummerStore.Core;
using SummerStore.Core.Models.Dto;
using SummerStore.Core.Models.Entities;
using SummerStore.Infrastructure.Data;

namespace SummerStore.Api.Endpoints;

public static class CartEndpoints
{
    public static IEndpointRouteBuilder MapCartEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/cart").RequireAuthorization();

        group.MapGet("/", GetCart);
        group.MapPost("/items", AddItem);
        group.MapPut("/items/{productId:guid}", UpdateItem);
        group.MapDelete("/items/{productId:guid}", RemoveItem);
        group.MapDelete("/", Clear);

        return app;
    }

    private static async Task<CartResponse?> LoadCart(AppDbContext db, Guid userId, CancellationToken ct)
    {
        var cart = await db.Carts.AsNoTracking()
            .Where(c => c.UserId == userId)
            .Select(c => new
            {
                c.Id,
                Items = c.Items
                    .OrderBy(i => i.CreatedAt)
                    .Select(i => new CartItemResponse(
                        i.ProductId, i.Product.Title, i.Product.Price,
                        i.Quantity, i.Product.Price * i.Quantity))
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);

        return cart is null
            ? null
            : new CartResponse(cart.Id, cart.Items, cart.Items.Sum(i => i.LineTotal));
    }

    private static async Task<IResult> GetCart(ClaimsPrincipal user, AppDbContext db, CancellationToken ct)
    {
        var cart = await LoadCart(db, user.GetUserId(), ct);
        return cart is null ? Results.NotFound() : Results.Ok(cart);
    }

    private static async Task<IResult> AddItem(
        AddCartItemRequest req, ClaimsPrincipal user,
        AppDbContext db, CancellationToken ct)
    {
        if (req.Quantity is < 1 or > 99)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["quantity"] = ["quantity is in range from 1 to 99"]
            });
        }

        var userId = user.GetUserId();

        var cart = await db.Carts
            .Include(c => c.Items.Where(i => i.ProductId == req.ProductId))
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);
        if (cart is null) return Results.NotFound();

        if (!await db.Products.AnyAsync(p => p.Id == req.ProductId, ct))
            return Results.NotFound(new { error = "Product not found" });

        var item = cart.Items.FirstOrDefault();
        if (item is null)
        {
            cart.Items.Add(new CartItem { ProductId = req.ProductId, Quantity = req.Quantity });
        }
        else
        {
            item.Quantity = Math.Min(item.Quantity + req.Quantity, 99);
        }

        await db.SaveChangesAsync(ct);
        return Results.Ok(await LoadCart(db, userId, ct));
    }

    private static async Task<IResult> UpdateItem(
        Guid productId, UpdateCartItemRequest req, ClaimsPrincipal user, AppDbContext db, CancellationToken ct)
    {
        if (req.Quantity is < 1 or > 99)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
            {
                ["quantity"] = ["quantity is in range from 1 to 99"]
            });
        }

        var userId = user.GetUserId();

        var item = await db.CartItems
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.Cart.UserId == userId, ct);
        if (item is null) return Results.NotFound();

        item.Quantity = req.Quantity;
        await db.SaveChangesAsync(ct);

        return Results.Ok(await LoadCart(db, userId, ct));
    }

    private static async Task<IResult> RemoveItem(
        Guid productId, ClaimsPrincipal user, AppDbContext db, CancellationToken ct)
    {
        var userId = user.GetUserId();

        var deleted = await db.CartItems
            .Where(i => i.ProductId == productId && i.Cart.UserId == userId)
            .ExecuteDeleteAsync(ct);

        return deleted == 0 ? Results.NotFound() : Results.Ok(await LoadCart(db, userId, ct));
    }

    private static async Task<IResult> Clear(ClaimsPrincipal user, AppDbContext db, CancellationToken ct)
    {
        var userId = user.GetUserId();
        await db.CartItems.Where(i => i.Cart.UserId == userId).ExecuteDeleteAsync(ct);
        return Results.NoContent();
    }
}