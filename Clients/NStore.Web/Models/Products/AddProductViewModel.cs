using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NStore.Web.Models.Shared;

namespace NStore.Web.Models.Products;

public class AddProductViewModel
{
    public string Name { get; set; }

    public string Description { get; set; }

    public ImageViewModel? Image { get; set; }

    public MoneyViewModel Price { get; set; }

    public int Amount { get; set; } = 1;

    public int CategoryId { get; set; }

    [ValidateNever]
    public SelectList Categories { get; set; }

    public AddProductViewModel()
    {
    }

    public AddProductViewModel(List<CategoryViewModel> categories)
    {
        if (categories == null || categories.Count == 0)
            return;

        Categories = new SelectList(categories.Select(x => new SelectListItem(x.Name, x.Id.ToString())), "Value", "Text");
    }
}
