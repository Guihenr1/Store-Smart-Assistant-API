using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Common.Pagination;
using StoreSmart.Application.Features.Products.DTOs;

namespace StoreSmart.Application.Features.Products.Queries;

public sealed record ListProductsQuery(
    int PageNumber = 1,
    int PageSize = 20,
    bool OnlyActive = true,
    string? Category = null,
    string? SearchTerm = null
) : IRequest<Result<PaginatedResponse<ProductResponse>>>;