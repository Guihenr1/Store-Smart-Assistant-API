using MediatR;
using StoreSmart.Api.Common;
using StoreSmart.Application.Features.Products.Commands;
using StoreSmart.Application.Features.Products.Queries;

namespace StoreSmart.Api.Endpoints;

public static class ProductEndpoints
{
    public static void RegisterProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products");

        // CREATE
        group.MapPost("/", async (CreateProductCommand command, IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result.ToMinimalApiResult();
            })
            .RequireAuthorization();

        // GET BY ID
        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetProductByIdQuery(id));
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization();

        // GET ALL
        // UPDATE (you can implement similarly)
        // DELETE (soft delete)
    }
}