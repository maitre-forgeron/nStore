namespace NStore.Web.Models.Products;

public class UpdateProductViewModel : AddProductViewModel
{
    public int Id { get; set; }

    public UpdateProductViewModel()
    {
    }

    public UpdateProductViewModel(ProductViewModel product, List<CategoryViewModel> categories) : base(categories)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Image = product.Image;
        Price = product.Price;
        Amount = product.Amount;
        CategoryId = product.CategoryId;
    }
}