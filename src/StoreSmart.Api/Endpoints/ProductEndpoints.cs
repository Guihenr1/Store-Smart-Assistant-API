using MediatR;
using StoreSmart.Api.Common;
using StoreSmart.Application.Features.Products.Commands;
using StoreSmart.Application.Features.Products.DTOs;
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
        group.MapGet("/", async (
            IMediator mediator,
            int pageNumber = 1,
            int pageSize = 20,
            bool onlyActive = true,
            string? category = null,
            string? searchTerm = null) =>
        {
            var query = new ListProductsQuery(pageNumber, pageSize, onlyActive, category, searchTerm);
            var result = await mediator.Send(query);
            return result.ToMinimalApiResult();
        })
        .RequireAuthorization();
        
        // UPDATE
        group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequest request, IMediator mediator) =>
            {
                var command = new UpdateProductCommand(id, request);
                var result = await mediator.Send(command);
                return result.ToMinimalApiResult();
            })
            .RequireAuthorization();

        // DELETE (Soft Delete)
        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var result = await mediator.Send(new DeleteProductCommand(id));
                return result.ToMinimalApiResult();
            })
            .RequireAuthorization();
    }
}