using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Models
{
    public class ProductsQuery : PagedQueryParams
    {
        [FromQuery(Name = "page")]
        public int? CategoryId { get; set; }
    }
}
