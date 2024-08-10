using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Models
{
    public class ProductsQuery : PagedQueryParams
    {
        [FromQuery]
        public int? CategoryId { get; set; }
    }
}
