using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Features.Products.DTOs;

namespace StoreSmart.Application.Features.Products.Commands;

public sealed record CreateProductCommand(CreateProductRequest Request) 
    : IRequest<Result<ProductResponse>>;