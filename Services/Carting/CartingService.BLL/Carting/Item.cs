using NStore.Shared.Exceptions;
using NStore.Shared.ValueObjects;

namespace CartingService.BLL.Carting
{
    public class Item
    {
        public int Id { get; private set; }

        public string Name { get; private set; }

        public Image Image { get; private set; }

        public Money Price { get; private set; }

        public int Quantity { get; private set; }

        private Item()
        {
        }

        public Item(int id, string name, int quantity, Image image, Money price)
        {
            Id = id;
            Name = name;
            Image = image;
            Price = price;
            Quantity = quantity;

            Validate(this);
        }

        public void Update(string name, Image image, Money price)
        {
            Name = name;
            Image = image;
            Price = price;

            Validate(this);
        }

        private static void Validate(Item item)
        {
            if (item.Id <= 0)
            {
                throw new DomainException(DomainErrorMessagesProvider.ProperyRequiredErrorMessage(nameof(Item), nameof(item.Id)), ExceptionLevel.Warning);
            }

            if (string.IsNullOrWhiteSpace(item.Name))
            {
                throw new DomainException(DomainErrorMessagesProvider.ProperyRequiredErrorMessage(nameof(Item), nameof(item.Name)), ExceptionLevel.Warning);
            }

            if(item.Quantity <= 0)
            {
                throw new DomainException(DomainErrorMessagesProvider.LessOrEqualToErrorMessage(nameof(Item), nameof(item.Quantity), "0"), ExceptionLevel.Warning);
            }

            if(item.Price == null)
            {
                throw new DomainException(DomainErrorMessagesProvider.ProperyRequiredErrorMessage(nameof(Item), nameof(item.Price)), ExceptionLevel.Warning);
            }
        }
    }
}
