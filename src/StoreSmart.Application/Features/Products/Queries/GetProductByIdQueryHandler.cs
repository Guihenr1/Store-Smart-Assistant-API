using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Features.Products.DTOs;
using StoreSmart.Application.Interfaces.Repositories;
using StoreSmart.Domain.Entities;

namespace StoreSmart.Application.Features.Products.Queries;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Result<ProductResponse>>
{
    private readonly IProductRepository _repository;

    public GetProductByIdQueryHandler(IProductRepository repository) => _repository = repository;

    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await _repository.GetByIdAsync(request.Id, ct);
        if (product == null)
            return Result.NotFound("Product not found");

        return Result.Success(MapToResponse(product));
    }

    private static ProductResponse MapToResponse(Product p) => new(
        p.Id, p.SKU, p.Name, p.Description, p.Brand, p.Category.Name,
        p.Price.Amount, p.Price.Currency, p.StockQuantity, p.IsActive,
        p.ImageUrl, p.Tags, p.Specifications, p.Features);
}