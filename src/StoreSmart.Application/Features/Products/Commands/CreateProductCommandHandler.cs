using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Features.Products.DTOs;
using StoreSmart.Application.Interfaces.Repositories;
using StoreSmart.Domain.Entities;
using StoreSmart.Domain.ValueObjects;

namespace StoreSmart.Application.Features.Products.Commands;

public sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductResponse>> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var req = request.Request;

        if (await _productRepository.SkuExistsAsync(req.SKU, cancellationToken))
        {
            return Result.Conflict("Product with SKU already exists.");
        }

        var category = Category.Create(req.Category);
        var price = Money.Create(req.Price);

        var product = Product.Create(
            sku: req.SKU,
            name: req.Name,
            description: req.Description,
            brand: req.Brand,
            category: category,
            price: price,
            stockQuantity: req.StockQuantity,
            imageUrl: req.ImageUrl,
            specifications: req.Specifications,
            features: req.Features
        );

        await _productRepository.AddAsync(product, cancellationToken);

        var response = new ProductResponse(
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

        return Result.Success(response);
    }
}