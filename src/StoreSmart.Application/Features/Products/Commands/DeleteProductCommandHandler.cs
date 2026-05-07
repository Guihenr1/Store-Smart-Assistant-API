using Ardalis.Result;
using MediatR;
using StoreSmart.Application.Interfaces.Repositories;

namespace StoreSmart.Application.Features.Products.Commands;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result>
{
    private readonly IProductRepository _repository;

    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var exists = await _repository.ExistsAsync(request.Id, ct);
        if (!exists)
            return Result.NotFound("Product not found");

        await _repository.DeleteAsync(request.Id, ct);

        return Result.Success();
    }
}