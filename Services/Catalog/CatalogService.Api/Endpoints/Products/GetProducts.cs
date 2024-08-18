using CatalogService.Api.Extensions;
using CatalogService.Api.Interfaces;
using CatalogService.Api.Models;
using CatalogService.Application.Dtos;
using CatalogService.Application.Products.Queries;
using MediatR;

namespace CatalogService.Api.Endpoints.Products;
public class GetProducts : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products", async ([AsParameters] ProductsQuery query, ISender mediator, HttpContext context, LinkGenerator linkGenerator) =>
        {
            var products = await mediator.Send(new GetProductsQuery(new GetProductsDto(query.Page, query.Limit, query.CategoryId)));
            context.Response.AddPaginationHeader(products, "GetProductsAsync", query, linkGenerator, context, true);

            return TypedResults.Ok(products);
        })
            .WithName("GetProductsAsync")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization("ClientIdPolicy");
    }
}
