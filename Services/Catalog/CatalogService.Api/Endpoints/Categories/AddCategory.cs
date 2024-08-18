using CatalogService.Api.Interfaces;
using CatalogService.Application.Categories.Commands;
using CatalogService.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Endpoints.Categories;

public class AddCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("categories", async Task<Results<Ok<int>, BadRequest>> ([FromBody] AddCategoryDto dto, ISender mediator) =>
        {
            if (dto == null || (dto != null && dto.ParentId.HasValue && dto.ParentId.Value < 0))
            {
                return TypedResults.BadRequest();
            }

            var categoryId = await mediator.Send(new AddCategoryCommand(dto));

            return TypedResults.Ok(categoryId);
        }).RequireAuthorization("ClientIdPolicy", "ManagerPolicy");
    }
}
