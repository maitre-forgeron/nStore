using CatalogService.Api.Interfaces;
using CatalogService.Application.Categories.Commands;
using CatalogService.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Endpoints.Categories;

public class UpdateCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("categories", async Task<Results<NoContent, BadRequest, NotFound>>([FromBody] UpdateCategoryDto dto, ISender mediator) =>
        {
            if (dto == null || (dto != null && dto.ParentId.HasValue && dto.ParentId.Value < 0))
            {
                return TypedResults.BadRequest();
            }

            var categoryId = await mediator.Send(new UpdateCategoryCommand(dto));

            if (categoryId == 0)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.NoContent();
        }).RequireAuthorization("ClientIdPolicy", "ManagerPolicy");
    }
}