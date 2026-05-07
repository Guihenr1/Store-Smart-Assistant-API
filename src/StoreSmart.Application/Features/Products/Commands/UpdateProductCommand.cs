using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Features.Products.DTOs;

namespace StoreSmart.Application.Features.Products.Commands;

public sealed record UpdateProductCommand(Guid Id, UpdateProductRequest Request) 
    : IRequest<Result<ProductResponse>>;