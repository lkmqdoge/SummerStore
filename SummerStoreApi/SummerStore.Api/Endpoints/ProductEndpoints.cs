using System.IO.Compression;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using SummerStore.Core;
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

        group.MapGet("/", GetList)
            .Produces<List<ProductListItem>>();

        group.MapGet("/{id:guid}", GetById)
            .Produces<ProductResponse>()
            .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/attachments", GetAttachments)
            .Produces<IFileHttpResult>();

        var admin = group.MapGroup("").RequireAuthorization();
        admin.MapPost("/", Create);
        admin.MapPut("/{id:guid}", Update);
        admin.MapDelete("/{id:guid}", Delete);
        admin.MapPost("{id:guid}/attachments", UploadFile)
            .DisableAntiforgery();

        // admin.MapPost("/{id:guid}/images", UploadImage).DisableAntiforgery();
        // admin.MapDelete("/{id:guid}/images/{imageId:guid}", DeleteImage);
    }

    private static async Task<IResult> GetList(AppDbContext db, CancellationToken ct)
    {
        var query = db.Products.AsNoTracking();
        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Select(p => new ProductListItem(
                p.Id, p.Title, p.Summary, p.Price,
                p.Attachments.Where(x => x.Type == Core.Enums.DocumentType.Preview).Select(i => (Guid?)i.Id).FirstOrDefault()))
            .ToListAsync(ct);

        return Results.Ok(items);
    }

    private static async Task<IResult> GetById(Guid id, AppDbContext db, CancellationToken ct)
    {
        var product = await db.Products.AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new ProductResponse(p.Id, p.Title, p.Summary,
                p.Description, p.Price, p.Attachments.Where(x => x.Type == Core.Enums.DocumentType.Preview).Select(i => i.Id).ToList()))
            .FirstOrDefaultAsync(ct);

        return product is null ? Results.NotFound() : Results.Ok(product);
    }


    private static async Task<IResult> Create(ProductRequest req, ClaimsPrincipal principal,
            AppDbContext db, CancellationToken ct)
    {
        if (Validate(req) is { } errors) return Results.ValidationProblem(errors);

        var product = new Product
        {
            Title = req.Title.Trim(),
            Summary = req.Summary.Trim(),
            Description = req.Description.Trim(),
            Price = req.Price,
            CreatorId = principal.GetUserId()
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

private static async Task<IResult> GetAttachments(
    Guid id,
    IOptions<StorageSettings> options,
    AppDbContext context,
    CancellationToken ct)
{
    var attachments = await context.Products
        .Where(p => p.Id == id)
        .SelectMany(p => p.Attachments)
        .AsNoTracking()
        .Select(a => new { a.FilePath, a.Name })
        .ToListAsync(ct);

    if (attachments.Count == 0) return Results.NotFound();

    var baseDir = Path.Combine(
        Path.GetFullPath(options.Value.AttachmentsRootPath), id.ToString());

    var tempPath = Path.Combine(Path.GetTempPath(), $"{id}.zip");

    try
    {
        await using (var fs = new FileStream(
            tempPath, FileMode.Create, FileAccess.Write, FileShare.None,
            bufferSize: 81920, useAsync: true))
        await using (var zip = new ZipArchive(fs, ZipArchiveMode.Create, leaveOpen: false))
        {
            foreach (var item in attachments)
            {
                var fullPath = Path.IsPathRooted(item.FilePath)
                    ? item.FilePath
                    : Path.Combine(baseDir, Path.GetFileName(item.FilePath));

                if (!File.Exists(fullPath)) continue;

                var entryName = string.IsNullOrWhiteSpace(item.Name)
                    ? Path.GetFileName(fullPath)
                    : item.Name;

                await zip.CreateEntryFromFileAsync(fullPath, entryName, CompressionLevel.Optimal);
            }
        } // ← zip закрыт, файл на диске целый

        return Results.File(
            tempPath,
            contentType: "application/zip",
            fileDownloadName: $"{id}.zip",
            enableRangeProcessing: true);
    }
    catch (OperationCanceledException)
    {
        TryDelete(tempPath);
        return Results.StatusCode(StatusCodes.Status499ClientClosedRequest);
    }
    catch
    {
        TryDelete(tempPath);
        // logger.LogError(ex, ...);
        return Results.Problem("Не удалось собрать архив");
    }
}

private static void TryDelete(string path)
{
    try { if (File.Exists(path)) File.Delete(path); } catch { /* ignore */ }
}
    public static async Task<IResult> UploadFile(
            [FromForm] UploadDocumentRequest request,
            IOptions<StorageSettings> options,
            ClaimsPrincipal principal,
            AppDbContext context,
            CancellationToken ct)
    {
        if (request.File is null || request.File.Length == 0)
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                    ["file"] = ["Файл обязателен"]
                    });
        }

        var userId = principal.GetUserId();

        var product = await context.Products.FirstOrDefaultAsync(x => x.Id == request.ProductId, ct);
        if (product is null) return Results.NotFound();
        if (product.CreatorId != userId) return Results.Forbid();

        var dir = Path.Combine(options.Value.AttachmentsRootPath, request.ProductId.ToString());
        Directory.CreateDirectory(dir);

        var safeName = Path.GetFileName(request.File.FileName);
        var fullPath = Path.Combine(dir, safeName);

        await using (var stream = File.Create(fullPath))
            await request.File.CopyToAsync(stream, ct);

        context.Documents.Add(new Document
                {
                Id = Guid.CreateVersion7(),
                ProductId = request.ProductId,
                Name = safeName,
                Size = request.File.Length,
                FilePath = Path.Combine(request.ProductId.ToString(), safeName),
                });
        await context.SaveChangesAsync(ct);

        return Results.Ok();
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