using NStore.Web.Models.Products;
using NStore.Web.Models.Shared;

namespace NStore.Web.Models.Carts;

public class AddItemViewModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ImageViewModel? Image { get; set; }

    public MoneyViewModel Price { get; set; }

    public int Quantity { get; set; }

    public AddItemViewModel(ProductViewModel product)
    {
        Id = product.Id;
        Name = product.Name;
        Image = product.Image;
        Price = product.Price;
        Quantity = 1;
    }
}