using Microsoft.AspNetCore.Http;

using SummerStore.Core.Enums;

namespace SummerStore.Core.Models.Dto;

public sealed record UploadDocumentRequest(
    Guid ProductId,
    DocumentType Type,
    IFormFile File
);
