using NStore.Web.Models.Shared;

namespace NStore.Web.Models.Carts
{
    public class ItemViewModel
    {
        public int Id { get; set; }

        public string CartId { get; set; }

        public string Name { get; set; }

        public ImageViewModel? Image { get; set; }

        public MoneyViewModel Price { get; set; }

        public int Quantity { get; set; }
    }
}
