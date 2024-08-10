using Microsoft.AspNetCore.Mvc;

namespace NStore.Web.Models.QueryParameters
{
    public class ProductsQuery : PagedQueryParams
    {
        [FromQuery]
        public int? CategoryId { get; set; }
    }
}
