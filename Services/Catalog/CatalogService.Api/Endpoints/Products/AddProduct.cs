using CatalogService.Api.Interfaces;
using CatalogService.Application.Dtos;
using CatalogService.Application.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Endpoints.Products;

public class AddProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products", async Task<Results<CreatedAtRoute<ProductDto>, BadRequest, Ok<ProductDto>>>([FromBody] AddProductDto dto, ISender mediator) =>
        {
            if (dto == null || (dto != null && dto.CategoryId < 0))
            {
                return TypedResults.BadRequest();
            }

            var product = await mediator.Send(new AddProductCommand(dto));

            return TypedResults.CreatedAtRoute(product, "GetProductAsync", new { id = product.Id });
        })
            .WithName("AddProductAsync")
            .RequireAuthorization("ClientIdPolicy", "ManagerPolicy");
    }
}
