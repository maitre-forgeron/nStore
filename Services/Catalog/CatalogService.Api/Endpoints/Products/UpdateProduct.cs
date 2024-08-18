using CatalogService.Api.Interfaces;
using CatalogService.Application.Dtos;
using CatalogService.Application.Products.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Endpoints.Products;

public class UpdateProduct : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("products", async Task<Results<NoContent, BadRequest, NotFound>> ([FromBody] UpdateProductDto dto, ISender mediator) =>
        {
            if (dto == null || (dto != null && dto.CategoryId <= 0))
            {
                return TypedResults.BadRequest();
            }

            var productId = await mediator.Send(new UpdateProductCommand(dto));

            if (productId == 0)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.NoContent();
        })
            .WithName("UpdateProductAsync")
            .RequireAuthorization("ClientIdPolicy", "ManagerPolicy");
    }
}
