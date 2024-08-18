using CatalogService.Api.Interfaces;
using CatalogService.Application.Categories.Queries;
using MediatR;

namespace CatalogService.Api.Endpoints.Categories;

public class GetCategories : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("categories", async (ISender mediator) =>
        {
            var categories = await mediator.Send(new GetCategoriesQuery());

            return TypedResults.Ok(categories);
        }).RequireAuthorization("ClientIdPolicy");
    }
}
