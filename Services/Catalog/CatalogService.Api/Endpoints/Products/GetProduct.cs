using CatalogService.Api.Interfaces;
using CatalogService.Application.Dtos;
using CatalogService.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CatalogService.Api.Endpoints.Products;

public class GetProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products/{id}", async Task<Results<Ok<ProductDto>, NotFound, BadRequest>> (int id, ISender mediator) =>
        {
            if (id <= 0)
            {
                return TypedResults.BadRequest();
            }

            var product = await mediator.Send(new GetProductQuery(id));

            if (product == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(product);
        })
            .WithName("GetProductAsync")
            .RequireAuthorization("ClientIdPolicy");
    }
}
