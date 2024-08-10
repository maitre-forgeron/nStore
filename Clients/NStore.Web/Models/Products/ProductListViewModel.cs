using NStore.Web.Models.Shared;

namespace NStore.Web.Models.Products;

public class ProductListViewModel
{
    public List<ProductViewModel> Products { get; set; }
    public PaginationViewModel Pagination { get; set; }
}
