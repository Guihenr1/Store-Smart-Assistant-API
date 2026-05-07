using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Common.Pagination;
using StoreSmart.Application.Features.Products.DTOs;
using StoreSmart.Application.Interfaces.Repositories;
using StoreSmart.Domain.Entities;

namespace StoreSmart.Application.Features.Products.Queries;

public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, Result<PaginatedResponse<ProductResponse>>>
{
    private readonly IProductRepository _repository;

    public ListProductsQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PaginatedResponse<ProductResponse>>> Handle(
        ListProductsQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(1, request.PageNumber);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);

        var (products, totalCount) = await _repository.GetPaginatedAsync(
            pageNumber,
            pageSize,
            request.OnlyActive,
            request.Category,
            request.SearchTerm,
            cancellationToken);

        var responses = products.Select(MapToResponse).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var response = new PaginatedResponse<ProductResponse>(
            Items: responses,
            PageNumber: pageNumber,
            PageSize: pageSize,
            TotalCount: totalCount,
            TotalPages: totalPages,
            HasNextPage: pageNumber < totalPages,
            HasPreviousPage: pageNumber > 1
        );

        return Result.Success(response);
    }

    private static ProductResponse MapToResponse(Product p) => new(
        p.Id, p.SKU, p.Name, p.Description, p.Brand, p.Category.Name,
        p.Price.Amount, p.Price.Currency, p.StockQuantity, p.IsActive,
        p.ImageUrl, p.Tags, p.Specifications, p.Features);
}