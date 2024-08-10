using NStore.Web.Models.Shared;

namespace NStore.Web.Models.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ImageViewModel? Image { get; set; }
        public MoneyViewModel Price { get; set; }
        public CategoryViewModel Category { get; set; }
        public int Amount { get; set; }
        public int CategoryId { get; set; }
    }
}
