using Ardalis.Result;
using MediatR;

namespace StoreSmart.Application.Features.Products.Commands;

public sealed record DeleteProductCommand(Guid Id) : IRequest<Result>;