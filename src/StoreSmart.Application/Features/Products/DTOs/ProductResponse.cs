namespace StoreSmart.Application.Features.Products.DTOs;

public record ProductResponse(
    Guid Id,
    string SKU,
    string Name,
    string Description,
    string Brand,
    string Category,
    decimal Price,
    string Currency,
    int StockQuantity,
    bool IsActive,
    string? ImageUrl,
    List<string> Tags,
    string? Specifications,
    string? Features);