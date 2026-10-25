using Microsoft.EntityFrameworkCore;

using SummerStore.Core.Models.Dto;
using SummerStore.Core.Models.Entities;
using SummerStore.Infrastructure.Data;

namespace SummerStore.Api.Endpoints;

public static class ProductEndpoints
{
    private const long MaxImageSize = 5 * 1024 * 1024;
    private static readonly string[] AllowedTypes = ["image/jpeg", "image/png", "image/webp"];

    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("api/products");

        group.MapGet("/", GetList);
        group.MapGet("/{id:guid}", GetById);
        group.MapGet("/{id:guid}/images/{imageId:guid}", GetImage);

        var admin = group.MapGroup("").RequireAuthorization();
        admin.MapPost("/", Create);
        admin.MapPut("/{id:guid}", Update);
        admin.MapDelete("/{id:guid}", Delete);
        admin.MapPost("/{id:guid}/images", UploadImage).DisableAntiforgery();
        admin.MapDelete("/{id:guid}/images/{imageId:guid}", DeleteImage);
    }

    private static async Task<IResult> GetList(AppDbContext db, CancellationToken ct)
    {
        var query = db.Products.AsNoTracking();
        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProductListItem(
                p.Id, p.Title, p.Summary, p.Price,
                p.Images.OrderBy(i => i.SortOrder).Select(i => (Guid?)i.Id).FirstOrDefault()))
            .ToListAsync(ct);

        return Results.Ok(items);
    }

    private static async Task<IResult> GetById(Guid id, AppDbContext db, CancellationToken ct)
    {
        var product = await db.Products.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductResponse(p.Id, p.Title, p.Summary,
                p.Description, p.Price, p.Images.OrderBy(i => i.SortOrder).Select(i => i.Id).ToList()))
            .FirstOrDefaultAsync(ct);

        return product is null ? Results.NotFound() : Results.Ok(product);
    }

    private static async Task<IResult> GetImage(Guid id, Guid imageId, AppDbContext db, CancellationToken ct)
    {
        var image = await db.ProductImages.AsNoTracking()
            .Where(i => i.Id == imageId && i.ProductId == id)
            .Select(i => new { i.Image, i.ContentType })
            .FirstOrDefaultAsync(ct);

        return image is null
            ? Results.NotFound()
            : Results.File(image.Image, image.ContentType);
    }

    private static async Task<IResult> Create(ProductRequest req, AppDbContext db, CancellationToken ct)
    {
        if (Validate(req) is { } errors) return Results.ValidationProblem(errors);

        var product = new Product
        {
            Title = req.Title.Trim(),
            Summary = req.Summary.Trim(),
            Description = req.Description.Trim(),
            Price = req.Price
        };

        db.Products.Add(product);
        await db.SaveChangesAsync(ct);

        return Results.Created($"/api/products/{product.Id}",
                new ProductResponse(product.Id, product.Title, product.Summary, product.Description, product.Price, []));
    }

    private static async Task<IResult> Update(Guid id, ProductRequest req, AppDbContext db, CancellationToken ct)
    {
        if (Validate(req) is { } errors) return Results.ValidationProblem(errors);

        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (product is null) return Results.NotFound();

        product.Title = req.Title.Trim();
        product.Summary = req.Summary.Trim();
        product.Description = req.Description.Trim();
        product.Price = req.Price;

        await db.SaveChangesAsync(ct);
        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid id, AppDbContext db, CancellationToken ct)
    {
        await using var tx = await db.Database.BeginTransactionAsync(ct);

        await db.CartItems.Where(ci => ci.ProductId == id).ExecuteDeleteAsync(ct);
        var deleted = await db.Products.Where(p => p.Id == id).ExecuteDeleteAsync(ct);

        await tx.CommitAsync(ct);
        return deleted == 0 ? Results.NotFound() : Results.NoContent();
    }

    private static async Task<IResult> UploadImage(
            Guid id, IFormFile file, AppDbContext db, CancellationToken ct)
    {
        if (file.Length is 0 or > MaxImageSize)
            return Results.BadRequest(new { error = "Размер файла должен быть от 1 байта до 5 МБ" });
        if (!AllowedTypes.Contains(file.ContentType))
            return Results.BadRequest(new { error = "Допустимы только JPEG, PNG и WebP" });

        if (!await db.Products.AnyAsync(p => p.Id == id, ct))
            return Results.NotFound();

        await using var ms = new MemoryStream((int)file.Length);
        await file.CopyToAsync(ms, ct);

        var maxOrder = await db.ProductImages
            .Where(i => i.ProductId == id)
            .Select(i => (int?)i.SortOrder)
            .MaxAsync(ct) ?? -1;

        var image = new ProductImage
        {
            Id = Guid.CreateVersion7(),
            ProductId = id,
            Image = ms.ToArray(),
            ContentType = file.ContentType,
            SortOrder = maxOrder + 1
        };

        db.ProductImages.Add(image);
        await db.SaveChangesAsync(ct);

        return Results.Created($"/api/products/{id}/images/{image.Id}", new { image.Id });
    }

    private static async Task<IResult> DeleteImage(Guid id, Guid imageId, AppDbContext db, CancellationToken ct)
    {
        var deleted = await db.ProductImages
            .Where(i => i.Id == imageId && i.ProductId == id)
            .ExecuteDeleteAsync(ct);

        return deleted == 0 ? Results.NotFound() : Results.NoContent();
    }

    private static Dictionary<string, string[]>? Validate(ProductRequest r)
    {
        var errors = new Dictionary<string, string[]>();

        if (string.IsNullOrWhiteSpace(r.Title) || r.Title.Length > 200)
            errors["title"] = ["Обязательное поле, до 200 символов"];
        if (string.IsNullOrWhiteSpace(r.Summary) || r.Summary.Length > 500)
            errors["summary"] = ["Обязательное поле, до 500 символов"];
        if (string.IsNullOrWhiteSpace(r.Description) || r.Description.Length > 4000)
            errors["description"] = ["Обязательное поле, до 4000 символов"];
        if (r.Price < 0)
            errors["price"] = ["Цена не может быть отрицательной"];

        return errors.Count > 0 ? errors : null;
    }
}

