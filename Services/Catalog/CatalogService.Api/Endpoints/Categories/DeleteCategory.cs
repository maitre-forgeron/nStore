using CatalogService.Api.Interfaces;
using CatalogService.Application.Categories.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CatalogService.Api.Endpoints.Categories;

public class DeleteCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("categories/{id}", async Task<Results<NoContent, BadRequest>> (int id, ISender mediator) =>
        {
            if (id <= 0)
            {
                return TypedResults.BadRequest();
            }

            await mediator.Send(new DeleteCategoryCommand(id));

            return TypedResults.NoContent();
        }).RequireAuthorization("ClientIdPolicy", "ManagerPolicy");
    }
}
