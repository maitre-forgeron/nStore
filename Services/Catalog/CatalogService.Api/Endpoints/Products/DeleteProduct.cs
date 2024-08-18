using CatalogService.Api.Interfaces;
using CatalogService.Application.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CatalogService.Api.Endpoints.Products;

public class DeleteProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("products/{id}", async Task<Results<NoContent, BadRequest>> (int id, ISender mediator) =>
        {
            if (id <= 0)
            {
                return TypedResults.BadRequest();
            }

            await mediator.Send(new DeleteProductCommand(id));

            return TypedResults.NoContent();
        }).RequireAuthorization("ClientIdPolicy", "ManagerPolicy");
    }
}
