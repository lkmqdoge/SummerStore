using Microsoft.AspNetCore.Http;

namespace SummerStore.Core.Models.Dto;

public sealed record UploadDocumentRequest(
    Guid ProductId,
    IFormFile File
);

