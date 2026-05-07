namespace StoreSmart.Application.Features.Products.DTOs;

public record UpdateProductRequest(
    string? Name,
    string? Description,
    decimal? Price,
    string? Brand,
    string? Category,
    string? CategoryDescription,
    string? Currency,
    bool? IsActive,
    List<string>? Tags,
    string? SKU,
    string? TechnicalDetails,
    int? StockQuantity,
    string? ImageUrl,
    string? Specifications,
    string? Features);