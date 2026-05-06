namespace StoreSmart.Application.Features.Products.DTOs;

public record UpdateProductRequest(
    string? Name,
    string? Description,
    decimal? Price,
    int? StockQuantity,
    string? ImageUrl,
    string? Specifications,
    string? Features);