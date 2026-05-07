using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Features.Products.Commands.Mappings;
using StoreSmart.Application.Features.Products.DTOs;
using StoreSmart.Application.Interfaces.Repositories;
using StoreSmart.Domain.ValueObjects;

namespace StoreSmart.Application.Features.Products.Commands;

public class UpdateProductCommandHandler(IProductRepository repository)
    : IRequestHandler<UpdateProductCommand, Result<ProductResponse>>
{
    public async Task<Result<ProductResponse>> Handle(
        UpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            return Result.NotFound("Product not found");

        var req = request.Request;

        product.Update(
            name: req.Name,
            description: req.Description,
            brand: req.Brand,
            price: req.Price.HasValue ? Money.Create(req.Price.Value) : null,
            stockQuantity: req.StockQuantity,
            imageUrl: req.ImageUrl,
            specifications: req.Specifications,
            features: req.Features
        );

        await repository.UpdateAsync(product, cancellationToken);

        return Result.Success(product.ToResponse());
    }
}