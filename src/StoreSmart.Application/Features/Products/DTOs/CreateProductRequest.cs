namespace StoreSmart.Application.Features.Products.DTOs;

public record CreateProductRequest(
    string SKU,
    string Name,
    string Description,
    string Brand,
    string Category,
    decimal Price,
    int StockQuantity,
    string? ImageUrl = null,
    string? Specifications = null,
    string? Features = null);