using StoreSmart.Application.Features.Products.DTOs;
using StoreSmart.Domain.Entities;

namespace StoreSmart.Application.Features.Products.Commands.Mappings;

public static class ProductMapping
{
    public static ProductResponse ToResponse(this Product product) => new(
        Id: product.Id,
        SKU: product.SKU,
        Name: product.Name,
        Description: product.Description,
        Brand: product.Brand,
        Category: product.Category.Name,
        Price: product.Price.Amount,
        Currency: product.Price.Currency,
        StockQuantity: product.StockQuantity,
        IsActive: product.IsActive,
        ImageUrl: product.ImageUrl,
        Tags: product.Tags,
        Specifications: product.Specifications,
        Features: product.Features
    );
}