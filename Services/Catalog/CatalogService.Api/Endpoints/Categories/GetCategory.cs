using CatalogService.Api.Interfaces;
using CatalogService.Application.Categories.Queries;
using CatalogService.Application.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CatalogService.Api.Endpoints.Categories;

public class GetCategory : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories/{id}", async Task<Results<Ok<CategoryDto>, NotFound, BadRequest>> (int id, ISender mediator) =>
        {
            if (id <= 0)
            {
                return TypedResults.BadRequest();
            }

            var category = await mediator.Send(new GetCategoryQuery(id));

            if (category == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(category);
        }).RequireAuthorization("ClientIdPolicy");
    }
}
