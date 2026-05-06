using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Features.Products.DTOs;

namespace StoreSmart.Application.Features.Products.Queries;

public sealed record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductResponse>>;